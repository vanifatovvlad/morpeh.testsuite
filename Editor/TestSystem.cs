using System;

namespace Morpeh.TestSuite.Editor {
    public abstract class TestSystemBase : ISystem {
        private Filter filter;

        public World World { get; set; }

        public Action<Filter> AwakeAction   { get; set; }
        public Action<Filter> DisposeAction { get; set; }
        public Action<Filter> UpdateAction  { get; set; }

        public void OnAwake() {
            this.filter = this.World.Filter.With<TestComponent>();
            this.AwakeAction?.Invoke(this.filter);
        }

        public void Dispose() {
            this.DisposeAction?.Invoke(this.filter);
        }

        public void OnUpdate(float deltaTime) {
            this.UpdateAction?.Invoke(this.filter);
        }

        public abstract void DoWorldUpdate(World world, float dt);
    }

    public class TestUpdateSystem : TestSystemBase {
        public override void DoWorldUpdate(World world, float dt) => world.Update(dt);
    }

    public class TestFixedSystem : TestSystemBase, IFixedSystem {
        public override void DoWorldUpdate(World world, float dt) => world.FixedUpdate(dt);
    }

    public class TestLateSystem : TestSystemBase, ILateSystem {
        public override void DoWorldUpdate(World world, float dt) => world.LateUpdate(dt);
    }
}