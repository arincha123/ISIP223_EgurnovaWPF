using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace Пр12
{
    public partial class basepart
    {
        public string FullDescription
        {
            get
            {
                try
                {
                    if (parttype.name == "CPU")
                    {
                        return $"{cpu.socket.name}, {cpu.numberofcores} × {cpu.basecorefrequency} ГГц, L3 - {cpu.cachel3} МБ, {cpu.igpu.name}, TDP {cpu.thermalpower} Вт";
                    }
                    else if (parttype.name == "GPU")
                    {
                        var videoconnectorslist = Core.Context.videoconnectorgpu
                            .Where(p => p.gpuid == gpu.id)
                            .Select(v => v.videoconnector.name)
                            .ToList();

                        string videoconnectors = "";
                        for (int i = 0; i < videoconnectorslist.Count; i++)
                        {
                            videoconnectors += videoconnectorslist[i];
                            if (i != videoconnectorslist.Count - 1)
                                videoconnectors += ", ";
                        }

                        return $"{gpu.gpuinterface.name}, GPU {gpu.chipfrequency} МГц, {gpu.videomemory} ГБ, {gpu.memorybus} бит, {videoconnectors}, {gpu.recommendpower} Вт";
                    }
                    else if (parttype.name == "RAM")
                    {
                        return $"{ram.memorytype.name}, {ram.capacity} ГБ × {ram.count} шт, {ram.ghz} МГц, тайминги {ram.timings}";
                    }
                    else if (parttype.name == "Motherboard")
                    {
                        return $"{motherboard.socket.name}, {motherboard.formfactor.name}, {motherboard.memoryslots}x{motherboard.memorytype.name}, " +
                            $"{motherboard.pcislots}xPCI-E, {motherboard.sataports}xSATA, {motherboard.usbports}xUSB";
                    }
                    else if (parttype.name == "Case")
                    {
                        var forms = Core.Context.boardformfactorcase
                            .Where(b => b.caseid == @case.id)
                            .Select(f => f.formfactor.name)
                            .ToList();

                        string formfactors = "";
                        for (int i = 0; i < forms.Count; i++)
                        {
                            formfactors += forms[i];
                            if (i != forms.Count - 1)
                                formfactors += ", ";
                        }

                        return $"{@case.casesize.name}, {formfactors}, слоты: {@case.expansionslots}, вентиляторы: {@case.fans}";
                    }
                    else if (parttype.name == "PowerSupply")
                    {
                        return $"{powersupply.power} Вт, {powersupply.fandimension.name} mm, {powersupply.certificate.name}";
                    }
                    else if (parttype.name == "ProcessorCooler")
                    {
                        var partsockets = Core.Context.socketprocessorcooler
                            .Where(s => s.processorcoolerid == processorcooler.id)
                            .Select(s => s.socket.name)
                            .ToList();

                        string sockets = "";
                        for (int i = 0; i < partsockets.Count; i++)
                        {
                            sockets += partsockets[i];
                            if (i != partsockets.Count - 1)
                                sockets += ", ";
                        }

                        return $"{sockets}, {processorcooler.fandimension.name}, теплотрубки: {processorcooler.heatpipes}, " +
                            $"{processorcooler.minspeed}-{processorcooler.maxspeed} об/мин, {processorcooler.noiselevel} дБ";
                    }
                    else
                    {
                        string res = "";

                        if (storagedevice.storagedevicetype.name == "SSD")
                            res = res + "SSD, " + storagedevice.ssd.tbw + " ТБ, ";
                        else
                            res += "HDD " + storagedevice.hdd.rotationspeed + " об/мин, ";

                        res += storagedevice.capacity + " ГБ, " + storagedevice.storagedeviceinterface.name;
                        return res;
                    }
                }
                catch
                {
                    return name;
                }
            }
        }
    }
}
