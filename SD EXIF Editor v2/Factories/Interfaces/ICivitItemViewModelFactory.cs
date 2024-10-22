﻿using SD_EXIF_Editor_v2.Model;
using SD_EXIF_Editor_v2.ViewModel.Interfaces;

namespace SD_EXIF_Editor_v2.Factories.Interfaces
{
    public interface ICivitItemViewModelFactory
    {
        public ICivitItemViewModel Create(CivitItem civitItem);
    }
}
