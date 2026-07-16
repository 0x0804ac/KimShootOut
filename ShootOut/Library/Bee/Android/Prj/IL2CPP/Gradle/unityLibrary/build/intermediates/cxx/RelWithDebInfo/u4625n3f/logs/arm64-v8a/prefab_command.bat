@echo off
"C:\\Program Files\\Unity\\Hub\\Editor\\6000.4.9f1\\Editor\\Data\\PlaybackEngines\\AndroidPlayer\\OpenJDK\\bin\\java" ^
  --class-path ^
  "C:\\Users\\user\\.gradle\\caches\\modules-2\\files-2.1\\com.google.prefab\\cli\\2.1.0\\aa32fec809c44fa531f01dcfb739b5b3304d3050\\cli-2.1.0-all.jar" ^
  com.google.prefab.cli.AppKt ^
  --build-system ^
  cmake ^
  --platform ^
  android ^
  --abi ^
  arm64-v8a ^
  --os-version ^
  24 ^
  --stl ^
  c++_shared ^
  --ndk-version ^
  27 ^
  --output ^
  "C:\\Users\\user\\AppData\\Local\\Temp\\agp-prefab-staging14887448587599166541\\staged-cli-output" ^
  "C:\\Users\\user\\.gradle\\caches\\9.1.0\\transforms\\1729aecbaee4d53c43e888c08a695309\\workspace\\transformed\\jetified-games-activity-4.4.0\\prefab"
