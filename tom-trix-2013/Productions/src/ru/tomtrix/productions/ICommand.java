package ru.tomtrix.productions;

/**
 * Интерфейс команды
 */
public interface ICommand {
    /**
     * Исполняет операцию (для операнда - сравнение, для посылки - логическая функция, для правила - вычисление посылки и означивание переменных и т.д.)
     */
    public boolean execute();
}
