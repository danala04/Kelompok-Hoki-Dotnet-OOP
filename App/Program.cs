using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace App
{
    public class Program
    {
        static List<PemberiPinjaman> databasePemberiPinjaman = new List<PemberiPinjaman>();

        static void Main(string[] args)
        {
            string menu = "";

            while (menu != "0")
            {
                Console.WriteLine("Selamat Datang di Peer-To-Peer Lending");
                Console.WriteLine("Apakah anda ingin menjadi:");
                Console.WriteLine("1. Peminjam");
                Console.WriteLine("2. Pemberi Pinjaman");
                Console.WriteLine("0. Keluar");
                Console.Write("Pilih(1,2,0): ");

                menu = Console.ReadLine();

                switch (menu)
                {
                    case "1":
                        HandlePeminjam();
                        break;
                    case "2":
                        HandlePemberiPinjaman();
                        break;
                    case "0":
                        Console.WriteLine("Terima kasih! Program selesai.");
                        break;
                    default:
                        Console.WriteLine("Pilihan tidak valid. Silakan pilih antara 1, 2, atau 0.");
                        break;
                }

                Console.WriteLine("");
            }
        }

        static void HandlePemberiPinjaman()
        {
            Console.WriteLine("Anda telah memilih untuk menjadi pemberi pinjaman");
            Console.Write("Masukan nama anda sebagai pemberi pinjaman: ");
            string namaPemberiPinjaman = Console.ReadLine();
            Console.Write("Masukan jumlah saldo yang ingin anda pinjamkan: ");
            double saldoPemberiPinjaman = Convert.ToDouble(Console.ReadLine());

            if(saldoPemberiPinjaman < 0)
            {
                Console.Write("Nominal Saldo Invalid!");
            }
            else
            {
                PemberiPinjaman pemberiPinjaman = new PemberiPinjaman(namaPemberiPinjaman, "Pemberi Pinjaman", saldoPemberiPinjaman);

                databasePemberiPinjaman.Add(pemberiPinjaman);
                Console.WriteLine($"{namaPemberiPinjaman} telah ditambahkan ke daftar pemberi pinjaman dengan saldo {ConvertSaldo(saldoPemberiPinjaman)}");
            }            
        }

        static void HandlePeminjam()
        {
            //List sementara
            List<PemberiPinjaman> dataPemberiPinjaman = new List<PemberiPinjaman>();

            Console.WriteLine("Anda telah memilih untuk menjadi peminjam");

            if (databasePemberiPinjaman.Count > 0)
            {
                Console.Write("Masukan nominal yang ingin anda pinjam: ");
                double nominalPinjaman = Convert.ToDouble(Console.ReadLine());

                if (nominalPinjaman < 0)
                {
                    Console.Write("Nominal Saldo Invalid!");
                }
                else
                {
                    //Detek Pemberi Peminjam yang saldonya lebih dari nominal
                    for (int i = 0; i < databasePemberiPinjaman.Count; i++)
                    {
                        PemberiPinjaman item = databasePemberiPinjaman[i];
                        if (item.Saldo >= nominalPinjaman)
                        {
                            dataPemberiPinjaman.Add(item);
                        }
                    }

                    if (dataPemberiPinjaman.Count > 0)
                    {
                        HandlePilihPeminjam(dataPemberiPinjaman, nominalPinjaman);
                    }
                    else
                    {
                        Console.WriteLine("Belum ada pemberi pinjaman yang menawarkan!");
                    }
                }                
            }
            else
            {
                Console.WriteLine("Belum ada pemberi pinjaman!");
            }
        }

        static void HandlePilihPeminjam(List<PemberiPinjaman> dataPemberiPinjaman, double nominalPinjaman)
        {
            double totalPembayaran=0;

            Console.WriteLine("List Peminjam:");
            for (int i = 0; i < dataPemberiPinjaman.Count; i++)
            {
                PemberiPinjaman item = databasePemberiPinjaman[i];
                Console.WriteLine($"{i + 1}. {item.Name} - Saldo: {ConvertSaldo(item.Saldo)}");
            }
            Console.Write("Pilih pemberi pinjaman (masukan nomor): ");
            int indexPemberiPinjaman = Convert.ToInt32(Console.ReadLine());

            PemberiPinjaman pemberiPinjaman = dataPemberiPinjaman[indexPemberiPinjaman-1];

            Console.WriteLine($"Anda telah meminjam {ConvertSaldo(pemberiPinjaman.Saldo)} dari {pemberiPinjaman.Name}");
            Console.WriteLine($"{pemberiPinjaman.Name} meminjamkan {ConvertSaldo(pemberiPinjaman.Saldo)} kepada Anda pada {DateTime.Now:dd MMMM yyyy HH:mm}. Pinjaman harus dibayar dalam 12 kali angsuran");

            Console.WriteLine("\nCicilan selama 12 bulan dengan bunga 2.5% per bulan:");
            for (int i = 1; i <= 12; i++)
            { double cicilanPerbulan = hitungCicilan(nominalPinjaman);
                Console.WriteLine($"Pembayaran {i}: {ConvertSaldo(cicilanPerbulan)}");
                totalPembayaran = totalPembayaran + cicilanPerbulan;
            }
            Console.WriteLine($"Total Pembayaran: {ConvertSaldo(totalPembayaran)}");

            for (int i = 0; i < databasePemberiPinjaman.Count; i++)
            {
                PemberiPinjaman item = databasePemberiPinjaman[i];
                if (item.Name == pemberiPinjaman.Name)
                {                    
                    databasePemberiPinjaman[i].Saldo -= nominalPinjaman;
                    break;  
                }
            }
        }

        static Double hitungCicilan(double nominalPinjaman)
        {

            double exponent = Math.Pow(1 + 0.025, -12);
            double denominator = 1 - exponent;
            double monthlyPayment = (0.025 * nominalPinjaman) / denominator;

            return monthlyPayment;
        }

        static String ConvertSaldo(double saldo)
        {
            string formattedAmount = string.Format(new CultureInfo("id-ID"), "Rp.{0:N2}", saldo);
            return formattedAmount;
        }
    }
}
