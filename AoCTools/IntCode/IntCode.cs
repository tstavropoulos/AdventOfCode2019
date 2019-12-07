using System;
using System.Collections.Generic;
using System.Text;

namespace AoCTools.IntCode
{
    public class IntCode
    {
        public int instr;
        public List<int> regs;
        private readonly Action<int> output;
        private readonly Func<int> input;
        public int lastOutput = 0;
        public bool done = false;

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
            Set = 3,
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

        public IntCode(IEnumerable<int> regs, Action<int> output = null, Func<int> input = null)
        {
            instr = 0;

            this.regs = new List<int>(regs);

            this.input = input;
            this.output = output;
        }

        public IntCode Clone() => new IntCode(regs);
        public IntCode CloneState() => new IntCode(regs) { instr = instr };

        private int GetValue(int input, bool mode)
        {
            if (mode)
            {
                return input;
            }

            return regs[input];
        }

        public int RunToOutput()
        {
            while (Execute() != State.Output) { }

            return lastOutput;
        }

        public State Execute()
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

                case Instr.Set:
                    regs[regs[instr + 1]] = input.Invoke();
                    instr += 2;
                    return State.Continue;

                case Instr.Output:
                    lastOutput = GetValue(regs[instr + 1], oneMode);
                    output?.Invoke(lastOutput);
                    instr += 2;
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
