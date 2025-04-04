using System.Collections;

namespace AllInOne
{
    public interface ICoroutineExecutable
    {
        void ExecuteCoroutine(IEnumerator routine);
    }
}