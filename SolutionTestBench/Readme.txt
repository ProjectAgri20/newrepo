The "PackageDeploymentSetup.iss" file is an Inno setup script that creates a distribution of files that can be used to then create an STB Client and STB Server installer for a target site.

The Package Deployment will put a directory of files in a location defined by you during installation.  Then within that distribution you can make modifications to the package before compiling the Client and Server Setup files.

The two primary changes you can make are:

1. You can decide which plugins will be enabled for the target installation.  This is done by editing the "PluginList.txt" file that is at the root of the package distribution.

2. You can add export files from STB to the "Import" folder.  You create these files using the STB console or Admin console pointing to your master system.  If you use the Admin Console you can only export Print Device entries or Test Document Library entries, STB User Console will additionally let you export test Scenarios.

If you choose to export data from your master system be sure to save those export files into the "Import" directory in the Deployment Package.  When you compile the Server setup these export files will be included.  When the Server Setup is executed these files will be imported into the new system as part of the installation process.  By adding and removing import files as well as modifying the PluginList you can customize your setup to meet your requirements for a target installation.

Also, note that the Deployment Package includes to Inno Setup installers in the "Inno Setup" folder.  These two installers must be installed on your system in order to build the Client and Server installers.  Here are some notes on installing Inno Setup:

Start with the ispack-5.5.5-unicode.exe installer first.  You can accept the defaults except for encryption.  Uncheck the box on the encryption page in the install wizard.

Once that is installed you can install idpsetup-1.5.0.  You can uncheck the Source Code option on this installer but accept defaults for the remaining wizard pages.


If you are installing the deployment package for the first time and don't already have an instance of STB up in your environment, you can use the package to build an initial server installer for your environment.  Because you don't have a master system already in place to use for source data, after you build and run your Server installer the first time your database will basically be empty.  It will have initial configuration data, but will not have any test scenarios, print device entries or test document entries.  You will need to begin adding those manually.



Check rights on file share.  It will need to be updated.



  