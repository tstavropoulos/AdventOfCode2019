using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading.Channels;

namespace AoCTools.IntCode
{
    public class IntCode
    {
        public long lastOutput = 0;
        public string Name { get; }

        private long instr;

        private bool done = false;

        private readonly Dictionary<long,long> regs;

        private long fixedInputIndex = 0;
        private readonly long[] fixedInputs;

        private readonly Action<long> output;
        private readonly Channel<long> inputChannel = Channel.CreateUnbounded<long>();

        private long relativeBase = 0;


        public enum State
        {
            Continue = 0,
            Output,
            Terminate
        }

        public enum Instr
        {
            Add = 1,
            Multiply = 2,
            Input = 3,
            Output = 4,
            JIT = 5,
            JIF = 6,
            LT = 7,
            EQ = 8,
            ADJ = 9,
            Terminate = 99
        }

        public enum Mode
        {
            position = 0,
            value,
            relative
        }

        public long this[int i]
        {
            get => regs[i];
            set => regs[i] = value;
        }

        public IntCode(
            string name,
            IEnumerable<long> regs,
            IEnumerable<long> fixedInputs = null,
            Action<long> output = null)
        {
            instr = 0;
            Name = name;

            this.regs = new Dictionary<long,long>(regs.Count());

            long index = 0;
            foreach(long value in regs)
            {
                this.regs.Add(index++, value);
            }

            this.fixedInputs = fixedInputs?.ToArray() ?? new long[0];
            this.output = output;
        }

        private long GetValue(long reg, Mode mode)
        {
            switch (mode)
            {
                case Mode.position:
                    return GetIndex(GetIndex(reg));

                case Mode.value:
                    return GetIndex(reg);

                case Mode.relative:
                    return GetIndex(GetIndex(reg) + relativeBase);

                default:
                    throw new Exception();
            }
        }

        private long GetIndex(long index)
        {
            if (regs.ContainsKey(index))
            {
                return regs[index];
            }

            return 0L;
        }


        private void SetValue(long reg, Mode setMode, long value)
        {
            switch (setMode)
            {
                case Mode.position:
                    regs[reg] = value;
                    break;

                case Mode.relative:
                    regs[relativeBase + reg] = value;
                    break;

                case Mode.value:
                default:
                    throw new Exception();
            }
        }

        public void WriteValue(long value)
        {
            inputChannel.Writer.WriteAsync(value);
        }

        public Task Run()
        {
            Task task = new Task(RunToEnd);
            task.Start();
            return task;
        }

        private void RunToEnd()
        {
            State state = State.Continue;
            while (state != State.Terminate)
            {
                Task<State> executeTask = Execute();
                executeTask.Wait();
                state = executeTask.Result;
            }
        }

        public async Task<State> Execute()
        {
            if (done)
            {
                return State.Terminate;
            }

            Instr instruction = (Instr)(regs[instr] % 100);
            Mode oneMode = (Mode)((regs[instr] / 100) % 10);
            Mode twoMode = (Mode)((regs[instr] / 1000) % 10);
            Mode threeMode = (Mode)((regs[instr] / 10000) % 10);

            switch (instruction)
            {
                case Instr.Add:
                    SetValue(regs[instr + 3], threeMode, GetValue(instr + 1, oneMode) + GetValue(instr + 2, twoMode));
                    instr += 4;
                    return State.Continue;

                case Instr.Multiply:
                    SetValue(regs[instr + 3], threeMode, GetValue(instr + 1, oneMode) * GetValue(instr + 2, twoMode));
                    instr += 4;
                    return State.Continue;

                case Instr.Input:
                    long inputValue;
                    if (fixedInputIndex < fixedInputs.Length)
                    {
                        inputValue = fixedInputs[fixedInputIndex++];
                    }
                    else
                    {
                        inputValue = await inputChannel.Reader.ReadAsync();
                    }
                    SetValue(regs[instr + 1], oneMode, inputValue);
                    instr += 2;
                    return State.Continue;

                case Instr.Output:
                    lastOutput = GetValue(instr + 1, oneMode);
                    instr += 2;
                    output?.Invoke(lastOutput);
                    return State.Output;

                case Instr.JIT:
                    if (GetValue(instr + 1, oneMode) != 0)
                    {
                        instr = GetValue(instr + 2, twoMode);
                    }
                    else
                    {
                        instr += 3;
                    }
                    return State.Continue;

                case Instr.JIF:
                    if (GetValue(instr + 1, oneMode) == 0)
                    {
                        instr = GetValue(instr + 2, twoMode);
                    }
                    else
                    {
                        instr += 3;
                    }
                    return State.Continue;

                case Instr.LT:
                    SetValue(regs[instr + 3], threeMode, GetValue(instr + 1, oneMode) < GetValue(instr + 2, twoMode) ? 1 : 0);
                    instr += 4;
                    return State.Continue;

                case Instr.EQ:
                    SetValue(regs[instr + 3], threeMode, GetValue(instr + 1, oneMode) == GetValue(instr + 2, twoMode) ? 1 : 0);
                    instr += 4;
                    return State.Continue;

                case Instr.ADJ:
                    relativeBase += GetValue(instr + 1, oneMode);
                    instr += 2;
                    return State.Continue;

                case Instr.Terminate:
                    done = true;
                    return State.Terminate;

                default: throw new Exception($"Unsupported instruction: {instruction}");
            }
        }

    }
}
