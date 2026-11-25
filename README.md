# LibrarySystem (ИС «Библиотека»)

Начальное окно приложения (главное меню) по макету Figma. Вариант 9.

## Структура решения
- `LibrarySystem.UI` - формы Windows Forms (View), корень композиции (`Program.cs`), навигация
- `LibrarySystem.Logic` - Presenters, интерфейсы View и навигации
- `LibrarySystem.Core` - модели (сущности); пока пусто
- `LibrarySystem.Data` - репозитории; пока пусто

## Шаблоны проектирования
MVP, Наблюдатель (события), Фабричный метод (`ScreenFactory`), внедрение зависимостей (`Program.cs`).

## Запуск
1. Откройте `LibrarySystem.sln` в Visual Studio 2026.
2. Назначьте `LibrarySystem.UI` запускаемым проектом (по умолчанию так и есть).
3. Нажмите F5.

Если у вас Visual Studio 2022 и .NET 8, замените `net10.0` на `net8.0` в трёх файлах
`.csproj` (Logic, Core, Data) и `net10.0-windows` на `net8.0-windows` в `LibrarySystem.UI.csproj`.
