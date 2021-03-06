# GantryCrane
Пример реализации универсальной системы внутриигрового взаимодействия для прототипа симулятора механики козлового крана 

## Архитектура
Архитектура пострена на абстрактных классах `SystemInput`, `Interactable`, `Device` и интерфейсе `INode`.

### [SystemInput](https://github.com/Ilya-ekb/GantryCrane/blob/main/Assets/Scripts/ManageScripts/SystemInput.cs)
`SystemInput` - используется для организации пользовательского ввода и взамодействия с объектами классов, наследующих от абстрактоного класса `Interactable` и реализует методы:
##### Абстрактные методы:
1. `OnAttach()` - вызывается при попытке взаимодействия с объектом и пытается получить доступ к ближайшему доступному в игровом объекту компонентом `Interactable`, при успехе может выполнить подготовку игрового объекта к дальнешему взаимодействию
2. `OnAttachedUpdate()` - вызывается каждый кадр пока происходит взаимодействие с объектом
3. `OnDetach()` - вызвается в последнем кадре перед окончаением взаимодействия с объектом, выполняет завершающие настройки объекта класса `Interactable`

### [Interactable](https://github.com/Ilya-ekb/GantryCrane/blob/main/Assets/Scripts/InteractableSystem/Interactable.cs)
`Interactable` - используется для перевода значений пользовательского ввода в нормализованный сигнал для передачи классам, наследующим от `Device` и реализует методы:
##### Абстрактные методы:
1. `UpdateObjectTrasform()` - обновление положения игрового объекта при взаимодействии
2. `ComputeSignal()` - расчет нормализованного сигнала, передаваемого как управляющий сигнала для наследников `Device`
3. `ComputeOutValue()` - расчет текущего значения `outValue` для установки положения игрового объекта
##### Виртуальные методы:
1. `InteractableBegin()` - стартовые настройки объекта при начале взаимодействия
2. `InteractableUpdate()` - вызов абстрактных методов и реализация внешних событий, подписанных на `UnityEvent.OnInteractableUpdate()`
3. `InteractableEnd()` - метод реализуемый перед последним кадром, в котором взаимодействие еще происходит
4. `ForceDisadle()` - принудительный вызов `UnityEvent.OnInteractableUpdate()` и установка `signal` в стартовое значение

### [Device](https://github.com/Ilya-ekb/GantryCrane/blob/main/Assets/Scripts/DeviceScripts/Device.cs)
`Device` - используется для перевода сигнальных значений от наследников классов `Interactable` в работу внутриигровых устройств:
##### Абстрактные методы:
1. `InitialSettings()` - начальные настройки устройства, вызывается в методе `Start()`
2. `Work()` - реализует работу устройства в зависимости от пришедшего управляющего сигнала

### [INode](https://github.com/Ilya-ekb/GantryCrane/blob/main/Assets/Scripts/ManageScripts/Node.cs)
`INode` - используется для реализации связи объектов классов `Interactable` с `Device`. Класс, реализующий интерфейс, должен имитировать рабочий узел из реальной жизни, который может включать множество контроллеров и множество устройств и управлять взаимодействием по принципу "каждый с каждым":
##### Методы:
1. `Connect()` - реализует подписку каждового класса `Device` на событие обновления каждого класса `Interactable` 
2. `Disconnect()` - удаление подписки
