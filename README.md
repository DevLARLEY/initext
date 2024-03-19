# initext
+ Extract key ids from an init.mp4 file
+ Supports Widevine and Playready. It can detect multiple PlayReady Header Records containing multiple key ids in one pssh.
+ 64-bit only.

# pyinitext
python version of initext: [here](https://github.com/DevLARLEY/initext)

# Usage
+ `initext_x64.exe <init.mp4 path>`

# Compile
+ Run `dotnet publish -r win-x64 -c Release -p:PublishAot=true` in the 'initext' directory

# Preview
+ Upper PSSH: Widevine
+ Lower PSSH: PlayReady
![Screenshot 2024-01-29 164626](https://github.com/DevLARLEY/initext/assets/121249322/ee544379-7994-4444-9cee-8b5acb613d09)
