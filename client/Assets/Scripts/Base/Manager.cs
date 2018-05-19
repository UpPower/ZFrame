
namespace Uppower
{
    public class Manager<T> where T: class, IManager, new()
    {
        T _instance = null;

        public T Instance {
            get {
                if (_instance == null) {
                    _instance = new T();
                }
                return _instance;
            }
        }

        public virtual void OnInit() 
        {

        }

        public virtual void OnDestroy() 
        {

        }

        public virtual void OnUpdate(float dt)
        {

        }
    }
}