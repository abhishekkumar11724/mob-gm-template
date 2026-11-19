using System.Threading;
using Cysharp.Threading.Tasks;
using Nimbus.Core;
using Newtonsoft.Json.Linq;

namespace Nimbus.Config;

public interface IConfigSource : IService
{
    string Name { get; }

    UniTask<Result<JObject>> GetConfigsAsync(CancellationToken cancellationToken);
}
