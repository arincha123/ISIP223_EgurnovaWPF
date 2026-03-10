using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Пр12
{
    static class CheckCompatibility
    {
        public static bool CanAddPart(List<basepart> existingParts, basepart newPart,
                              out string errorMessage, out basepart partToReplace)
        {
            errorMessage = "";
            partToReplace = null;

            if (newPart.parttype.name != "RAM" && newPart.parttype.name != "StorageDevice")
            {
                partToReplace = existingParts.FirstOrDefault(p => p.parttypeid == newPart.parttypeid);

                if (partToReplace != null)
                {
                    var otherParts = existingParts.Where(p => p.parttypeid != newPart.parttypeid).ToList();
                    return CheckCompatibilityWithOthers(otherParts, newPart, out errorMessage);
                }
            }

            return CheckCompatibilityWithOthers(existingParts, newPart, out errorMessage);
        }

        private static bool CheckCompatibilityWithOthers(List<basepart> otherParts, basepart newPart, out string errorMessage)
        {
            errorMessage = "";

            if (newPart.parttype.name == "CPU")
            {
                var motherboard = otherParts.FirstOrDefault(p => p.parttype.name == "Motherboard")?.motherboard;
                if (motherboard != null && newPart.cpu.socketid != motherboard.socketid)
                {
                    errorMessage = $"Сокет процессора ({newPart.cpu.socket.name}) не подходит к материнской плате ({motherboard.socket.name})";
                    return false;
                }

                var cooler = otherParts.FirstOrDefault(p => p.parttype.name == "ProcessorCooler")?.processorcooler;
                if (cooler != null)
                {
                    bool coolerSupports = Core.Context.socketprocessorcooler
                        .Any(sc => sc.processorcoolerid == cooler.id && sc.socketid == newPart.cpu.socketid);
                    if (!coolerSupports)
                    {
                        errorMessage = $"Кулер не поддерживает сокет {newPart.cpu.socket.name}";
                        return false;
                    }
                }
            }

            else if (newPart.parttype.name == "Motherboard")
            {
                var cpu = otherParts.FirstOrDefault(p => p.parttype.name == "CPU")?.cpu;
                if (cpu != null && cpu.socketid != newPart.motherboard.socketid)
                {
                    errorMessage = $"Сокет материнской платы ({newPart.motherboard.socket.name}) не подходит к процессору ({cpu.socket.name})";
                    return false;
                }

                var case_ = otherParts.FirstOrDefault(p => p.parttype.name == "Case")?.@case;
                if (case_ != null)
                {
                    bool caseSupports = Core.Context.boardformfactorcase
                        .Any(bf => bf.caseid == case_.id && bf.formfactorid == newPart.motherboard.formfactorid);
                    if (!caseSupports)
                    {
                        errorMessage = $"Корпус не поддерживает форм-фактор {newPart.motherboard.formfactor.name}";
                        return false;
                    }
                }

                var ram = otherParts.FirstOrDefault(p => p.parttype.name == "RAM")?.ram;
                if (ram != null && ram.memorytypeid != newPart.motherboard.memorytypeid)
                {
                    errorMessage = $"Материнская плата ({newPart.motherboard.memorytype.name}) не совместима с памятью ({ram.memorytype.name})";
                    return false;
                }
            }

            else if (newPart.parttype.name == "RAM")
            {
                var motherboard = otherParts.FirstOrDefault(p => p.parttype.name == "Motherboard")?.motherboard;
                if (motherboard != null)
                {
                    if (newPart.ram.memorytypeid != motherboard.memorytypeid)
                    {
                        errorMessage = $"Память ({newPart.ram.memorytype.name}) не подходит к материнской плате ({motherboard.memorytype.name})";
                        return false;
                    }

                    int ramCount = otherParts.Count(p => p.parttype.name == "RAM");
                    if (ramCount >= motherboard.memoryslots)
                    {
                        errorMessage = $"На материнской плате только {motherboard.memoryslots} слотов памяти";
                        return false;
                    }
                }
            }

            else if (newPart.parttype.name == "GPU")
            {
                var ps = otherParts.FirstOrDefault(p => p.parttype.name == "PowerSupply")?.powersupply;
                if (ps != null && newPart.gpu.recommendpower.HasValue && ps.power < newPart.gpu.recommendpower)
                {
                    errorMessage = $"Блок питания {ps.power}Вт слишком слаб для видеокарты (нужно {newPart.gpu.recommendpower}Вт)";
                    return false;
                }
            }

            else if (newPart.parttype.name == "ProcessorCooler")
            {
                var cpu = otherParts.FirstOrDefault(p => p.parttype.name == "CPU")?.cpu;
                if (cpu != null)
                {
                    bool coolerSupports = Core.Context.socketprocessorcooler
                        .Any(sc => sc.processorcoolerid == newPart.processorcooler.id && sc.socketid == cpu.socketid);
                    if (!coolerSupports)
                    {
                        errorMessage = $"Кулер не поддерживает сокет {cpu.socket.name}";
                        return false;
                    }
                }
            }

            else if (newPart.parttype.name == "Case")
            {
                var mb = otherParts.FirstOrDefault(p => p.parttype.name == "Motherboard")?.motherboard;
                if (mb != null)
                {
                    bool caseSupports = Core.Context.boardformfactorcase
                        .Any(bf => bf.caseid == newPart.@case.id && bf.formfactorid == mb.formfactorid);
                    if (!caseSupports)
                    {
                        errorMessage = $"Корпус не поддерживает форм-фактор {mb.formfactor.name}";
                        return false;
                    }
                }
            }

            else if (newPart.parttype.name == "PowerSupply")
            {
                var gpu = otherParts.FirstOrDefault(p => p.parttype.name == "GPU")?.gpu;
                if (gpu != null && gpu.recommendpower.HasValue && newPart.powersupply.power < gpu.recommendpower)
                {
                    errorMessage = $"Блок питания {newPart.powersupply.power}Вт слишком слаб для видеокарты (нужно {gpu.recommendpower}Вт)";
                    return false;
                }
            }

            return true;
        }
    }

}