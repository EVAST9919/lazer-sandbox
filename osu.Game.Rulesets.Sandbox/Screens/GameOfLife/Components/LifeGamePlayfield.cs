using System;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Shaders;
using osu.Framework.Utils;
using osu.Game.Rulesets.Sandbox.Graphics;

namespace osu.Game.Rulesets.Sandbox.Screens.GameOfLife.Components
{
    public class LifeGamePlayfield : ShaderContainer
    {
        public double UpdateDelay { set; get; } = 200;

        private readonly bool[] cells;
        private readonly bool[] lastGenCells;

        private readonly int sizeX;
        private readonly int sizeY;

        public LifeGamePlayfield(int sizeX, int sizeY)
            : base("lifegame")
        {
            this.sizeX = sizeX;
            this.sizeY = sizeY;

            cells = new bool[sizeX * sizeY];
            lastGenCells = new bool[sizeX * sizeY];

            RelativeSizeAxes = Axes.Both;
        }

        protected override void LoadComplete()
        {
            base.LoadComplete();
            CreateRandomMap();
        }

        public void Pause()
        {
            Scheduler.CancelDelayedTasks();
        }

        public void Continue()
        {
            Scheduler.AddDelayed(onNewUpdate, UpdateDelay);
        }

        public void CreateRandomMap()
        {
            ClearMap();

            for (int i = 0; i < cells.Length; i++)
                cells[i] = RNG.NextBool();
        }

        public void ClearMap()
        {
            for (int i = 0; i < cells.Length; i++)
            {
                lastGenCells[i] = false;
                cells[i] = false;
            }
        }

        private void onNewUpdate()
        {
            saveLastGeneration();

            for (int i = 0; i < cells.Length; i++)
            {
                int x = i < sizeX ? i : i % sizeX;
                int y = i / sizeX;

                int nearbyCellsAmount = countNeighbours(x, y);

                if (!lastGenCells[i] && nearbyCellsAmount == 3)
                {
                    cells[i] = true;
                    continue;
                }

                if (lastGenCells[i] && !(nearbyCellsAmount == 2 || nearbyCellsAmount == 3))
                    cells[i] = false;
            }

            Scheduler.AddDelayed(onNewUpdate, UpdateDelay);
        }

        private void saveLastGeneration()
        {
            for (int i = 0; i < cells.Length; i++)
                lastGenCells[i] = cells[i];
        }

        private int countNeighbours(int x, int y)
        {
            int amount = 0;

            ////
            int checkableX = x - 1;
            int checkableY = y - 1;

            if (checkableX < 0)
                checkableX = sizeX - 1;
            if (checkableY < 0)
                checkableY = sizeY - 1;

            if (lastGenCells[getArrayIndex(checkableX, checkableY)])
                amount++;

            ////

            if (lastGenCells[getArrayIndex(x, checkableY)])
                amount++;

            ////

            checkableX = x + 1;

            if (checkableX > sizeX - 1)
                checkableX = 0;

            if (lastGenCells[getArrayIndex(checkableX, checkableY)])
                amount++;

            ////

            checkableX = x - 1;

            if (checkableX < 0)
                checkableX = sizeX - 1;

            if (lastGenCells[getArrayIndex(checkableX, y)])
                amount++;

            ////

            checkableX = x + 1;

            if (checkableX > sizeX - 1)
                checkableX = 0;

            if (lastGenCells[getArrayIndex(checkableX, y)])
                amount++;

            ////

            checkableX = x - 1;
            checkableY = y + 1;

            if (checkableX < 0)
                checkableX = sizeX - 1;
            if (checkableY > sizeY - 1)
                checkableY = 0;

            if (lastGenCells[getArrayIndex(checkableX, checkableY)])
                amount++;

            ////

            if (lastGenCells[getArrayIndex(x, checkableY)])
                amount++;

            ////

            checkableX = x + 1;

            if (checkableX > sizeX - 1)
                checkableX = 0;

            if (lastGenCells[getArrayIndex(checkableX, checkableY)])
                amount++;
            ////

            return amount;
        }

        private int getArrayIndex(int x, int y) => y * sizeY + x;

        protected override ShaderDrawNode CreateShaderDrawNode() => new LifeGameDrawNode(this);

        private class LifeGameDrawNode : ShaderDrawNode
        {
            protected new LifeGamePlayfield Source => (LifeGamePlayfield)base.Source;

            private Data data;

            public LifeGameDrawNode(LifeGamePlayfield source)
                : base(source)
            {
            }

            public override void ApplyState()
            {
                base.ApplyState();

                data = new Data
                {
                    State = Source.cells
                };
            }

            protected override void UpdateUniforms(IShader shader)
            {
                shader.GetUniform<Data>("data").UpdateValue(ref data);
            }

            private struct Data : IEquatable<Data>
            {
                public bool[] State;

                public bool Equals(Data other) => State == other.State;
            }
        }
    }
}
