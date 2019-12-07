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
        public int lastOutput = 0;
        public string Name { get; }

        private int instr;

        private bool done = false;

        private readonly int[] regs;

        private int fixedInputIndex = 0;
        private readonly int[] fixedInputs;

        private readonly Action<int> output;
        private readonly Channel<int> inputChannel = Channel.CreateUnbounded<int>();

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
            Terminate = 99
        }

        public int this[int i]
        {
            get => regs[i];
            set => regs[i] = value;
        }

        public IntCode(
            string name,
            IEnumerable<int> regs,
            IEnumerable<int> fixedInputs = null,
            Action<int> output = null)
        {
            instr = 0;
            Name = name;

            this.regs = regs.ToArray();
            this.fixedInputs = fixedInputs?.ToArray() ?? new int[0];
            this.output = output;
        }

        private int GetValue(int input, bool mode)
        {
            if (mode)
            {
                return input;
            }

            return regs[input];
        }

        public void WriteValue(int value)
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
            bool oneMode = ((regs[instr] / 100) % 10) == 1;
            bool twoMode = ((regs[instr] / 1000) % 10) == 1;
            bool threeMode = ((regs[instr] / 10000) % 10) == 1;

            switch (instruction)
            {
                case Instr.Add:
                    regs[regs[instr + 3]] = GetValue(regs[instr + 1], oneMode) + GetValue(regs[instr + 2], twoMode);
                    instr += 4;
                    return State.Continue;

                case Instr.Multiply:
                    regs[regs[instr + 3]] = GetValue(regs[instr + 1], oneMode) * GetValue(regs[instr + 2], twoMode);
                    instr += 4;
                    return State.Continue;

                case Instr.Input:
                    int inputValue;
                    if (fixedInputIndex < fixedInputs.Length)
                    {
                        inputValue = fixedInputs[fixedInputIndex++];
                    }
                    else
                    {
                        inputValue = await inputChannel.Reader.ReadAsync();
                    }
                    regs[regs[instr + 1]] = inputValue;
                    instr += 2;
                    return State.Continue;

                case Instr.Output:
                    lastOutput = GetValue(regs[instr + 1], oneMode);
                    instr += 2;
                    output?.Invoke(lastOutput);
                    return State.Output;

                case Instr.JIT:
                    if (GetValue(regs[instr + 1], oneMode) != 0)
                    {
                        instr = GetValue(regs[instr + 2], twoMode);
                    }
                    else
                    {
                        instr += 3;
                    }
                    return State.Continue;

                case Instr.JIF:
                    if (GetValue(regs[instr + 1], oneMode) == 0)
                    {
                        instr = GetValue(regs[instr + 2], twoMode);
                    }
                    else
                    {
                        instr += 3;
                    }
                    return State.Continue;

                case Instr.LT:
                    regs[regs[instr + 3]] = GetValue(regs[instr + 1], oneMode) < GetValue(regs[instr + 2], twoMode) ? 1 : 0;
                    instr += 4;
                    return State.Continue;

                case Instr.EQ:
                    regs[regs[instr + 3]] = GetValue(regs[instr + 1], oneMode) == GetValue(regs[instr + 2], twoMode) ? 1 : 0;
                    instr += 4;
                    return State.Continue;

                case Instr.Terminate:
                    done = true;
                    return State.Terminate;

                default: throw new Exception($"Unsupported instruction: {instruction}");
            }
        }

    }
}
