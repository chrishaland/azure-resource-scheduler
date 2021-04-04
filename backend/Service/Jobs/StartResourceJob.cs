﻿using Azure.ResourceManager.Compute;
using Azure.ResourceManager.Compute.Models;
using Repository.Models;
using System.Threading;
using System.Threading.Tasks;
using Resource = Repository.Models.Resource;

namespace Service.Jobs
{
    public class StartResourceJob
    {
        private readonly ComputeManagementClient _client;

        public StartResourceJob(ComputeManagementClient client)
        {
            _client = client;
        }

        public async Task Execute(Resource resource, CancellationToken ct)
        {
            switch(resource.Kind)
            {
                case ResourceKind.VirtualMachine:
                    await StartVirtualMachine(resource, ct);
                    break;
                case ResourceKind.NodePool:
                    await StartNodePool(resource, ct);
                    break;
                default:
                    break;
            }
        }

        private async Task StartVirtualMachine(Resource resource, CancellationToken ct = default)
        {
            await _client.VirtualMachines.StartStartAsync(
                resourceGroupName: resource.ResourceGroup,
                vmName: resource.Name,
                cancellationToken: ct
           );
        }

        private async Task StartNodePool(Resource resource, CancellationToken ct)
        {
            var vmss = await _client.VirtualMachineScaleSets.GetAsync(
                resourceGroupName: resource.ResourceGroup,
                vmScaleSetName: resource.Name,
                cancellationToken: ct
            );

            vmss.Value.Sku.Capacity = resource.NodePoolCount;

            var parameters = new VirtualMachineScaleSetUpdate
            {
                Sku = vmss.Value.Sku
            };

            await _client.VirtualMachineScaleSets.StartUpdateAsync(
                resourceGroupName: resource.ResourceGroup,
                vmScaleSetName: resource.Name,
                parameters: parameters,
                cancellationToken: ct
            );
        }
    }
}
