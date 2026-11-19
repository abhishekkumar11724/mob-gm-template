using System.Threading;
using Cysharp.Threading.Tasks;

namespace Nimbus.Core;

public interface IInitializable
{
    UniTask InitializeAsync(CancellationToken cancellationToken);
}
