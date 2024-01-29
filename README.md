# initext
+ Extract key ids from an init.mp4 file
+ Supports Widevine and Playready. It can detect multiple PlayReady Header Records containing multiple key ids in one pssh.
+ 64-bit only.

# Usage
+ `initext_x64.exe <init.mp4 path>`

# Compile
+ Run `dotnet publish -r win-x64 -c Release -p:PublishAot=true` in the 'initext' directory

# Preview
![image](https://github.com/DevLARLEY/initext/assets/121249322/b2ee8166-9737-47ea-99bf-22a857293e14)
