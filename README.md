# SD EXIF Editor v2

Tiny tool for viewing and editing image metadata associated with [AUTOMATIC1111/stable-diffusion-webui](https://github.com/AUTOMATIC1111/stable-diffusion-webui) generation parameters

New version with greatly improved visuals and functionality

# Demo

<img src="https://github.com/PetrK39/SD-EXIF-Editor-v2/blob/master/Documentation/demo.gif" width="700" />

# Features

- Edit raw metadata of your image
- Delete metadata from your image
- [civit.ai](https://civit.ai/)-like UI
- Quickly open or download models from the image by clicking on it

# Usage

Open the .png file through this application in a convenient way
- Use command line
- Drop the file on `SD EXIF Editor.exe` or on link to it
- (Recommened) Use FastStone ImageViewer and add `SD EXIF Editor.exe` as External Program. Then use `E` shortcut

# In case of errors

I'm 100% sure that I'm missing some rare cases and unusual metadata properties from the webui
In case you can't see some properties or program says about error, please create an issue with error codes and your raw metadata.
I'll try my best to fix it.

# In case of you want to help

Feel free to create pull request if you can introduce useful features
I also need a help with making programm cross-platform. Let me know if you can code and test cross-platform approaches for following modules:
- `System.Windows.MessageBox` replacement in `Service.MessageService`
- `System.Windows.Clipboard` replacement in `ViewModel.MainViewModel.Copy()`
- `System.Diagnostics.Process.Start` replacement in `ViewModel.MainViewModel.OpenUri()`