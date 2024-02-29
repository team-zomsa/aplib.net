namespace Aplib.Core
{
    public interface IState
    {
        public void Update(Observation observation);
    }
}