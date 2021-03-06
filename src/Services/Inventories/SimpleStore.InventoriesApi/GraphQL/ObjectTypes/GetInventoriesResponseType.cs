﻿using HotChocolate.Types;
using SimpleStore.Inventories.Infrastructure.EfCore.Options;
using SimpleStore.Inventories.Infrastructure.EfCore.UseCases.GetInventories;

namespace SimpleStore.InventoriesApi.GraphQL.ObjectTypes
{
    public class GetInventoriesResponseType : ObjectType<GetInventoriesResponse>
    {
        #region Overrides of ObjectType<GetInventoriesResponse>

        protected override void Configure(IObjectTypeDescriptor<GetInventoriesResponse> descriptor)
        {
            descriptor.Name($"{nameof(ServiceOptions.InventoriesApi)}_{nameof(GetInventoriesResponse)}");
        }

        #endregion
    }
}
