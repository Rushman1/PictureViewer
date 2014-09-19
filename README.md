PictureViewer
=============

WPF Picture Viewer

This repository holds a Picture Viewer that I use on a daily basis to view images from my hard drive.

It is a WPF 4.5 application written with VS 2013 and C# and uses .Net version 4.5.1.

The solution is divided into different projects:
   - Business - holds the models and other business related pieces of code
   - Infrastructure - holds the global classes like: 
      -  Enums 
      -  Helpers 
      -  Images 
      -  Interfaces 
      -  ViewModelBase
   - Modules.ImageModule - the work horse of the application and holds the ImageViewer.xaml user control
   - PictureViewer_V2 - WPF Application to hold the ImageModules user control
   - Resources - Holds images that are used in the application as well as styles resource dictionaries
   - Services.DataAccessService - (Future) holds the access to a data store to get images
      Move current way to get the directory of images to here
   - Services.GlobalParametersService - holds settings information global to the application
   - Services.LogService - used to log error information
   - Services.SettingsWindowService - WPF user control for settings like:
      - Directory of images (Folder)
      - Shuffle
      - Include subfolders
      - Length to show each picture
      - Transition between pictures (Using transitionals version 1.2.0.0)

The application is written with MVVM in mind and uses MS Prism/Unity to load the user controls.
It also contains a 'Debug Viewer' that currently shows a listbox of debugging information found throughout the code.
The listbox itemssource can be exported to a file for viewing

Future enhancements:
  1. Move folder searching/listing to DataAccess project
  2. Connect to tumblr.com, photobucket.com, flickr.com, a web service of images...
  3. Fade colored border from color to color.  Currently this border just changes.  I want to figure out how to 
     make the color changing fluid.
  4. Enhance the Debug Window to be able to list only the image names for comparing and checking the shuffling logic
  5. Create log viewer similar to the Debug Window (or include it into the Debug Window) so that any error logs
     can be viewed without having to go to Windows Explorer and opening in notepad
  6. A test suite for testing functionality (I know, I know, I should have started with that, but I wanted to write it
     and get it working without having to learn TDD)
     
     
That's all I have for now.  If you look at my code and see a way to improve it, just send me a note! I am open to enhancement suggestions as well.

Hope you enjoy the application and if you do, please send me an email and let me know!

Thanks,

Tim
