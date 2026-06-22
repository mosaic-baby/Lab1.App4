using System;
using System.Windows;
using System.Windows.Controls;

namespace Lab1App4
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            ЗаполнитьГодаИМесяцы();
        }

        // Этот метод запускается один раз при старте программы
        private void ЗаполнитьГодаИМесяцы()
        {
            // 1. Заполняем годы циклами for от текущего вниз до 1900 года
            int текущийГод = DateTime.Now.Year;
            for (int год = текущийГод; год >= 1900; год--)
            {
                ComboYear.Items.Add(год);
            }

            // 2. Заполняем месяцы от 1 до 12
            for (int месяц = 1; месяц <= 12; месяц++)
            {
                ComboMonth.Items.Add(месяц);
            }
        }

        // Этот метод срабатывает ТОГДА, когда пользователь выбирает ЧТО-ТО в списке года или месяца
        private void ComboYearMonth_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            // Проверяем: выбрал ли пользователь и год, и месяц одновременно?
            if (ComboYear.SelectedItem != null && ComboMonth.SelectedItem != null)
            {
                // Достаем выбранные значения из списков
                int выбранныйГод = (int)ComboYear.SelectedItem;
                int выбранныйМесяц = (int)ComboMonth.SelectedItem;

                // Очищаем старые дни в списке перед добавлением новых
                ComboDay.Items.Clear();

                // Встроенный метод C# сам считает, сколько дней в конкретном месяце конкретного года
                int количествоДней = DateTime.DaysInMonth(выбранныйГод, выбранныйМесяц);

                // Заполняем выпадающий список дней с помощью цикла for
                for (int день = 1; день <= количествоДней; день++)
                {
                    ComboDay.Items.Add(день);
                }

                // Делаем список дней АКТИВНЫМ для пользователя
                ComboDay.IsEnabled = true;

                // Сбрасываем текст результата, так как дата еще не выбрана до конца
                TextResult.Text = "Теперь выберите день.";
            }
            else
            {
                // Если что-то не выбрано — блокируем дни назад
                ComboDay.IsEnabled = false;
                ComboDay.Items.Clear();
            }
        }

        // Этот метод срабатывает, когда пользователь выбирает конкретный День
        private void ComboDay_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            // Защита: если список дней очищался программно, SelectedItem будет равен null
            if (ComboDay.SelectedItem == null) return;

            // Достаем все выбранные данные
            int год = (int)ComboYear.SelectedItem;
            int месяц = (int)ComboMonth.SelectedItem;
            int день = (int)ComboDay.SelectedItem;

            // Создаем объект выбранной даты
            DateTime выбраннаяДата = new DateTime(год, месяц, день);
            DateTime текущаяДата = DateTime.Now;

            // Защита: если выбранная дата из будущего
            if (выбраннаяДата > текущаяДата)
            {
                TextResult.Text = "Вы выбрали дату из будущего!";
                return;
            }

            // Простой алгоритм подсчета разницы вручную (без сложных библиотек)
            int летПрошло = текущаяДата.Year - выбраннаяДата.Year;
            int месяцевПрошло = текущаяДата.Month - выбраннаяДата.Month;
            int днейПрошло = текущаяДата.Day - выбраннаяДата.Day;

            // Корректируем дни, если день текущей даты меньше дня рождения
            if (днейПрошло < 0)
            {
                месяцевПрошло--; // "Занимаем" один месяц

                // Вычисляем предыдущий календарный месяц для корректного заема дней
                int прошлыйМесяц = текущаяДата.Month == 1 ? 12 : текущаяДата.Month - 1;
                int прошлыйГод = текущаяДата.Month == 1 ? текущаяДата.Year - 1 : текущаяДата.Year;

                int днейВПрошломМесяце = DateTime.DaysInMonth(прошлыйГод, прошлыйМесяц);
                днейПрошло += днейВПрошломМесяце;
            }

            // Корректируем месяцы, если ушли в минус
            if (месяцевПрошло < 0)
            {
                летПрошло--;
                месяцевПрошло += 12;
            }

            // Выводим результат в красивом виде на экран
            TextResult.Text = $"С выбранной даты прошло:\n" +
                              $"{летПрошло} лет, {месяцевПрошло} месяцев и {днейПрошло} дней.";
        }
    }
}