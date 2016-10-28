namespace BeatTheComputer.GUI
{
    public class ObjectWrapper<T> where T : class
    {
        private T reference;

        public ObjectWrapper(T reference)
        {
            this.reference = reference;
        }

        public T Reference {
            get { return reference; }
            set { reference = value; }
        }
    }
}
