using UnityEngine;

public class WeaponSwitcher : MonoBehaviour
{
    [SerializeField] private GameObject[] weaponModels;

    void Start()
    {
        // При старте включаем первое оружие (индекс 0)
        // и выключаем все остальные
        SwitchWeapon(0);
    }

    void Update()
    {
        // Проверяем нажатие клавиш 1, 2
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            SwitchWeapon(0); // Оружие 1
        }
        else if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            SwitchWeapon(1); // Оружие 2
        }
        else if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            SwitchWeapon(2); // Оружие 3
        }

    }

    void SwitchWeapon(int index)
    {
        // Проверяем, что такой индекс есть в массиве
        if (index < 0 || index >= weaponModels.Length)
        {
            return;
        }

        // 1. Прячем ВСЕ модели оружия
        for (int i = 0; i < weaponModels.Length; i++)
        {
            if (weaponModels[i] != null)
            {
                weaponModels[i].SetActive(false);
            }
        }

        // 2. Показываем ОДНУ нужную модель
        if (weaponModels[index] != null)
        {
            weaponModels[index].SetActive(true);
        }
    }
}
