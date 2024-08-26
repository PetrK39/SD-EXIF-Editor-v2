using Microsoft.Extensions.DependencyInjection;
using SD_EXIF_Editor_v2.Factories.Interfaces;
using SD_EXIF_Editor_v2.Model;
using SD_EXIF_Editor_v2.ViewModels.Interfaces;
using System;

namespace SD_EXIF_Editor_v2.Factories
{
    public class CivitItemViewModelFactory : ICivitItemViewModelFactory
    {
        private readonly IServiceProvider serviceProvider;
        public CivitItemViewModelFactory(IServiceProvider serviceProvider)
        {
            this.serviceProvider = serviceProvider;
        }
        public ICivitItemViewModel Create(CivitItem civitItem)
        {
            var vm = serviceProvider.GetRequiredService<ICivitItemViewModel>();
            if (vm is not CivitItemViewModel civitVM) return vm;
            civitVM.Initialize(civitItem);
            return civitVM;
        }
    }
}
