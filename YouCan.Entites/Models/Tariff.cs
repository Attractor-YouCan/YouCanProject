﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YouCan.Entities;

namespace YouCan.Entites.Models;

public class Tariff : EntityBase
{
    public int Id { get; set; }
    public string Name { get; set; }
    public int Price { get; set; }
    public int? Duration { get; set; } // длительность тарифа обозначается в месяцах: тариф pro длится 1 месяц 
                                       // тариф premium - 3 месяца
                                       // у user будет связь на тариф, а также новое поле - дата окончания тарифа
                                       // когда он подключает тариф то к дате плюсуется длительность тарифа
                                       // Пример реализации
                                       // user.TariffEndDate = user.TariffEndDate.AddMonths(user.Tariff.Duration)
}