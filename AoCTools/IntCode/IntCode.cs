using System;
using System.Collections.Generic;
using System.Text;

namespace AoCTools.IntCode
{
    public class IntCode
    {
        public int instr;
        public int[] regs;

        public enum State
        {
            Continue = 0,
            Terminate
        }

        public enum Instr
        {
            Add = 1,
            Multiply = 2,
            Terminate = 99
        }

        public int this[int i]
        {
            get => regs[i];
            set => regs[i] = value;
        }

        public IntCode(int[] regs, bool clone = false)
        {
            instr = 0;

            if (clone)
            {
                this.regs = (int[])regs.Clone();
            }
            else
            {
                this.regs = regs;
            }
        }

        public IntCode Clone() => new IntCode(regs, true);
        public IntCode CloneState() => new IntCode(regs, true) { instr = instr };

        public State Execute()
        {
            switch ((Instr)regs[instr])
            {
                case Instr.Add:
                    regs[regs[instr + 3]] = regs[regs[instr + 1]] + regs[regs[instr + 2]];
                    instr += 4;
                    return State.Continue;

                case Instr.Multiply:
                    regs[regs[instr + 3]] = regs[regs[instr + 1]] * regs[regs[instr + 2]];
                    instr += 4;
                    return State.Continue;

                case Instr.Terminate:
                    return State.Terminate;

                default: throw new Exception($"Unsupported instruction: {regs[instr]}");
            }
        }

    }
}
