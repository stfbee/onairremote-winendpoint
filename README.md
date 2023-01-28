# On Air Remote
v 0.1.0

## Что это?
Windows-сервер, отвечающий на вопрос "а есть ли работающий микрофон?"

## Сборка
Из корня директории выполнить

`dotnet publish '.\On Air Endpoint.sln' --self-contained false -c Release -p:PublishReadyToRun=true`

В папке `bin\Release\net7.0\win-x64` будет лежать все необходимое:
* onairendpoint.dll
* onairendpoint.exe
* onairendpoint.runtimeconfig.json

## Установка
Собрать, положить рядом dll из проекта https://github.com/stfbee/onairremote-winlib, запустить.

Для работы понадобится либо запустить `onairendpoint.exe` от имени администратора, либо выполнив команду 

```netsh http add urlacl url="http://+:4200/" user=everyone```

Дополнительно необходимо разрешить все входящие соиденения на 4200 порт в файрволе

## TODO
1. Выбор порта для ендпоинта
2. Объединение C# и C++ проектов в один
3. Вещание адреса/инфы на манер IoT
4. Разобраться с демоном (докер?)
5. Разобраться с файрволом
6. CI