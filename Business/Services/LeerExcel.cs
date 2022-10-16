using NPOI.SS.UserModel;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using Web.Business.Interfaces;
using Web.Data;
using Web.Models;

namespace Web.Business.Services
{
    public class LeerExcel : ILeerExcel
    {
        private readonly IUnitOfWork _unitOfWork;
        public LeerExcel(IUnitOfWork unitOfWork)
        {
            this._unitOfWork = unitOfWork;
        }

        public void Leerexcel(IWorkbook MiExcel)
        {
            ISheet HojaExcel = MiExcel.GetSheetAt(0);

            int cantidadFilas = HojaExcel.LastRowNum;
            List<string> cells = new List<string>();

            for (int Ffila = 0; Ffila <= cantidadFilas; Ffila++)
            {
                IRow fila = HojaExcel.GetRow(Ffila);

                if (Ffila != 0 && fila.Cells[0].ToString() == "Código")
                {

                    Cliente cliente = new Cliente()
                    {
                        IdentificacionFiscal = cells[2] == "Null" ? "NULL" : cells[2],
                        IdFormaPago = cells[3] == "Null" ? 5 : int.Parse(cells[3].Split('-')[0]),
                        NombreComercial = cells[6] == "Null" ? "NULL" : cells[6],
                        Domicilio = cells[8] == "Null" ? "NULL" : cells[8],
                        CodigoPostal = cells[10] == "Null" ? "NULL" : cells[10],
                        IdTipoCliente = cells[11] == "Null" ? 6 : int.Parse(cells[11].ToString().Substring(1)),
                        Poblacion = cells[12] == "Null" ? "NULL" : cells[12],
                        IdIdentificacionFiscal = 1,
                        Provincia = cells[14] == "Null" ? "NULL" : cells[14],
                        Movil = cells[20] == "Null" ? "NULL" : cells[20],
                        IdActividad = cells[21] == "Null" ? 1 : int.Parse(cells[21].ToString().Substring(1)),
                        Iva = cells[23] == "Null" ? false : cells[23].ToLower() == "si" ? false : true,
                        FechaAlta = cells[30] == "Null" ? DateTime.Now : DateTime.Parse(cells[30])
                    };

                    _unitOfWork.ClienteRepository.Add(cliente);
                    _unitOfWork.Save();

                    //ClienteEmail clienteMail = new ClienteEmail()
                    //{
                    //    IdCliente = cliente.CodigoCliente,
                    //    Email = cells[26],
                    //    Activo = true
                    //};

                    //_unitOfWork.ClienteEmailRepository.Add(clienteMail);
                    _unitOfWork.Save();

                    cells.Clear();

                }

                if (fila.GetCell(1) != null)
                    cells.Add(fila.GetCell(1).RichStringCellValue.ToString());
                else
                    cells.Add("Null");

                if (fila.GetCell(3) != null)
                    cells.Add(fila.GetCell(3).RichStringCellValue.ToString());
                else
                    cells.Add("Null");
            }


        }


    }
}
