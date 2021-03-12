using System;
using System.Diagnostics;
using System.Threading;

namespace isletim__9hafta
{
    class Program
    {//rase conditions çözülmemiş hali
     //    private static int sp;
     //    private static int sinir;

        //    static void toplambul()
        //    {
        //        for (int i = 0; i < 1000000; i++)
        //        {

        //            sp++;//bu problemde global değişkenimiz sp
        //        }

        //        sinir++;
        //    }
        //    static void Main(string[] args)
        //    {
        //      for(int i=0;i<10;i++)//birden fazla thread eşzamanlı olarak spnin değerini arttırıyor.Bu nedenden süre çok fazla çıktı
        //        {
        //            Thread t = new Thread(toplambul);
        //                t.Start();
        //        }
        //            Stopwatch sw = new Stopwatch();
        //            sw.Start();
        //            while (sinir < 10)
        //                sw.Stop();
        //            Console.WriteLine(sw.ElapsedMilliseconds);
        //            Console.WriteLine(sp);

        //    }

        // }
        //-------------------------------------LCK ÇÖZÜMÜ
        //    private static int sp;
        //    private static int sinir;

        //        static int lck = 0;//bu çözmünün adı lock variables olarak geçer
        //        static void toplambul()
        //    {
        //        for (int i = 0; i < 2000000; i++)
        //        {
        //                while (lck == 1) ;//threadlerdne sadece bir tanesi aktif işleme geçiyor.
        //                //diğerleri bekliyor
        //                lck = 1;//biz burda aslında lck ile bir kritik alan daha eklemiş olduk
        //                //problemimizi çözemedik.Bu problemi yazılım ile değil donanım ile çözüyoruz
        //                //cpudan yardım alıyoruz. Cpunun atomik komutları vardır.
        //                //atomik komut tamamlanmadan iş yapılmaz.
        //                //cpu atomik komut çalıştırıken ramde hafıza bölümünü kitliyor
        //                //başka thread veya core hafızaya ulaşamıyor.
        //                //lckyı donanıma söylüyoruz ve donanım hafızadaki bu bölgeyi
        //                //atomik komt olarak belirliyor. ve aynı anda orada sadece bir threadın işlem
        //                //yapmasını sağlıyor. böylece bizim kritik alana girişimiz güzelce kontrol ediliyor.
        //                //
        //                sp++;//bu problemde global değişkenimiz sp burası krititk alan
        //                lck = 0;
        //        }

        //        sinir++;
        //    }
        //    static void Main(string[] args)
        //    {
        //        for (int i = 0; i < 10; i++)
        //        {
        //            Thread t = new Thread(toplambul);
        //            t.Start();
        //        }
        //        Stopwatch sw = new Stopwatch();
        //        sw.Start();
        //        while (sinir < 10)
        //            sw.Stop();
        //        Console.WriteLine(sw.ElapsedMilliseconds);
        //        Console.WriteLine(sp);

        //    }
        //}
        //------------------------------problem çözümü-----------------------
        //    private static int sp;
        //    private static int sinir;
        //    static object obj = new object();


        //    static void toplambul()
        //    {
        //        for (int i = 0; i < 2000000; i++)
        //        {

        //            lock (obj)//objeyi kilitledik ve kritik alanı şekillendirdik.burda atomik komutları kullanarak çözdük
        //            {
        //                sp++;
        //            }
        //        }

        //        sinir++;
        //    }
        //    static void Main(string[] args)
        //    {
        //        for (int i = 0; i < 10; i++)
        //        {
        //            Thread t = new Thread(toplambul);
        //            t.Start();
        //        }
        //        Stopwatch sw = new Stopwatch();
        //        sw.Start();
        //        while (sinir < 10)
        //            sw.Stop();
        //        Console.WriteLine(sw.ElapsedMilliseconds);
        //        Console.WriteLine(sp);

        //    }
        //}
        //---------------------YUKARIDAKİ ÇÖZÜM İLE PERFORMANS HIZIMIZ YAVAŞLADI LCK SAYESİNDE BUNU HIZLANDIRMAK İÇİN;
        //EN İYİ VE EN HIZLI ÇÖZÜMÜ BUDUR.
        //    private static int sp;
        //    private static int sinir;

        //    static void toplambul()
        //    {
        //        for (int i = 0; i < 2000000; i++)
        //        {
        //            Interlocked.Increment(ref sp);//sp++ demektir aslında sp değerini arttırır.Sıradaki core u bekletir. Hafıza bölgesine birden fazla thread müdahale
        //            //etmek istediğinde atomik bir komut olan interlocked sınıfının increment komutu
        //            //şu anki aktif komut haric diğerlerini durdurur. Arttırma işlemi bittikten sonra diğeri gelir sırayla
        //            //sp değişkenini kendimiz kapatmış olduk.
        //            sp++;
        //        }

        //        sinir++;
        //    }
        //    static void Main(string[] args)
        //    {
        //        for (int i = 0; i < 10; i++)
        //        {
        //            Thread t = new Thread(toplambul);
        //            t.Start();
        //        }
        //        Stopwatch sw = new Stopwatch();
        //        sw.Start();
        //        while (sinir < 10)
        //            sw.Stop();
        //        Console.WriteLine(sw.ElapsedMilliseconds);
        //        Console.WriteLine(sp);

        //    }

        //}
        //------------------KRİTİK ALANA BİZ KENDİMİZ SIRAYLA GİRMEK İÇİN NASIL BİR ÇÖZÜM ÜRETMELİYİZ-----------------
        //    private static int sp;
        //    private static int sinir;
        //    static int lck = 0;//lck burda atomik bir komut değil bu arada 

        //    static void toplambul()
        //    {
        //        for (int i = 0; i < 1000000; i++)
        //        {
        //            kilitle();//kritik alan kilitledik


        //           // lock(obj)
        //           // {//kilitle
        //                sp++;//kritik alan
        //           // }//kilitçöz
        //            kilitcoz();
        //        }

        //        sinir++;
        //    }
        //    static void kilitle()
        //    {
        //        while (Interlocked.Exchange(ref lck, 1) == 1) ;//lck değişkenine 1i yüklemeyi sağlar
        //           // lck = 1;
        //    }
        //    static void kilitcoz()
        //    {
        //        lck = 0;
        //    }
        //    static void Main(string[] args)
        //    {
        //        for (int i = 0; i < 10; i++)
        //        {
        //            Thread t = new Thread(toplambul);
        //            t.Start();
        //        }
        //        Stopwatch sw = new Stopwatch();
        //        sw.Start();
        //        while (sinir < 10)
        //            sw.Stop();
        //        Console.WriteLine(sw.ElapsedMilliseconds);
        //        Console.WriteLine(sp);

        //    }

        //}
        //    //-------------------------SEMPHORE İÇİN GEÇERLİ OLAN KİLİTLEME İSE;--------------
        //    private static int sp;
        //    private static int sinir;
        //    static int lck = 0;//lck burda atomik bir komut değil bu arada 

        //    static void toplambul()
        //    {
        //        for (int i = 0; i < 1000000; i++)
        //        {
        //            kilitle();


        //            sp++;//kritik alan

        //            kilitcoz();
        //        }

        //        sinir++;
        //    }
        //    static void kilitle()
        //    {
        //        while (lck >= 5)
        //            lck++;
        //    }
        //    static void kilitcoz()
        //    {
        //        lck--;
        //    }
        //    static void Main(string[] args)
        //    {
        //        for (int i = 0; i < 10; i++)
        //        {
        //            Thread t = new Thread(toplambul);
        //            t.Start();
        //        }
        //        Stopwatch sw = new Stopwatch();
        //        sw.Start();
        //        while (sinir < 10)
        //            sw.Stop();
        //        Console.WriteLine(sw.ElapsedMilliseconds);
        //        Console.WriteLine(sp);

        //    }

        //}
        //    //-------------------------SEMPHORE İÇİN BİR ÖRNEK;--------------
        //    private static int sp;
        //    private static int sinir;
        //    static int lck = 0;//lck burda atomik bir komut değil bu arada 

        //    static void toplambul()
        //    {
        //        for (int i = 0; i < 1000000; i++)
        //        {
        //            kilitle();


        //            sp++;//kritik alan

        //            kilitcoz();
        //        }

        //        sinir++;
        //    }
        //    static void kilitle()
        //    {
        //        while (lck >= 5)
        //            lck++;
        //    }
        //    static void kilitcoz()
        //    {
        //        lck--;
        //    }

        //    private static Semaphore _pool;//semaphor olusturduk
        //    private static int _padding;
        //    private static void Worker(object num)
        //    {

        //        Console.WriteLine("Thread {0} begins " +
        //            "and waits for the semaphore.", num);
        //        _pool.WaitOne();//bekleme moduna geçer.kritik alan girme
        //        //5 thread burada bekler.yarım saniye uyur. Herhangi 3 tanesi içeri giriş yaptı.
        //        //geriye kalan 2 threadin girmesi threadlerin çıkmasına bağlı. bir thread çıkarda bekleyen
        //        //biri daha girer. biri daha çıkınca biri daha girer.
        //        int padding = Interlocked.Add(ref _padding, 100);//kritik alan baslangıc

        //        Console.WriteLine("Thread {0} enters the semaphore.", num);


        //        Thread.Sleep(10000 + padding);

        //        Console.WriteLine("Thread {0} releases the semaphore.", num);//kritik alan bitiş
        //       _pool.Release();//kritik alandan çıkma

        //}
        //static void Main(string[] args)
        //    {
        //        _pool = new Semaphore(0, 3);//semaphoreı create ettik.burdaki 3 kısmı aynı and akaç
        //        //kaynağı kritik alana alacak onu belirler burda 3 kaynak alınacak
        //        //bu kısım 1 olsaydı olsaydı semaphor otomatik olraka lock gibi çalışırdı.
        //        //0 ise kritik alana şuan kimseyi alma demek.Bu değer 1 olsaydı hemen 1 kaynağı alacaktı kritik alana.


        //        for (int i = 1; i <= 5; i++)//5 thread olusturuldu
        //        {
        //            Thread t = new Thread(Worker);

        //            t.Start(i);//threade bir parametre vermis
        //        }

        //        //not:hangi threadin sisteme ilk gireceği konusunda bir garanti yoktur.
        //        Thread.Sleep(5000);

        //        Console.WriteLine("Main thread calls Release(3).");
        //        _pool.Release(3);

        //        Console.WriteLine("Main thread exits.");
        //    }
        //}
        //-------------------------SEMPHORE İÇİN DİĞER ÖRNEK;--------------
        private static int sp;
        private static int sinir;
        static int lck = 0;//lck burda atomik bir komut değil bu arada 

        static void toplambul()
        {
            for (int i = 0; i < 1000000; i++)
            {
                kilitle();


                sp++;//kritik alan

                kilitcoz();
            }

            sinir++;
        }
        static void kilitle()
        {
            while (lck >= 5)
                lck++;
        }
        static void kilitcoz()
        {
            lck--;
        }

        private static Semaphore _pool;//semaphor olusturduk
        private static int _padding;
        private static void Worker(object num)
        {

            Console.WriteLine("Thread {0} begins " +
                "and waits for the semaphore.", num);
            _pool.WaitOne();//bekleme moduna geçer.kritik alan girme.
            //5 thread burada bekler.yarım saniye uyur. Herhangi 3 tanesi içeri giriş yaptı.
            //geriye kalan 2 threadin girmesi threadlerin çıkmasına bağlı. bir thread çıkarda bekleyen
            //biri daha girer. biri daha çıkınca biri daha girer.


            int padding = Interlocked.Add(ref _padding, 100);//kritik alan baslangıc
            Console.WriteLine("Thread {0} enters the semaphore.", num);
            Thread.Sleep(10000 + padding);
            Console.WriteLine("Thread {0} releases the semaphore.", num);//kritik alan bitiş
            _pool.Release();//kritik alandan çıkma

        }
        static object l1 = new object();
        static object l2 = new object();
        static void Main(string[] args)
        {
            Thread t = new Thread(()=> {
                lock (l1)
                {
                    Console.WriteLine("1");
                    Thread.Sleep(1000);

                    lock (l2) Console.WriteLine("2");
                }
              
                    });
            t.Start();
            lock(l2)
            {
                Console.WriteLine("3");
                Thread.Sleep(1000);
                lock (l1) Console.WriteLine("4");
            }
        }//BU YAZDIKLARIMIZIN EKRAN ÇIKTISI NEDİR???
        // 1 VE 3 YAZDIRDI.ÇÜNKÜ TAKILI KALDIK. BU DURUMA DEADLOCK DENİR.TÜRKÇESİ ÖLÜMCÜL KİLİT.
        //SENKRONİZASYON NESNELERİ PROGRAMIN KİLİTLENMESİNE YOL AÇTI.NASIL KURTULACAĞIZ BU DURUMDAN?
        //GERİ DÖNÜŞ YOKTUR.DEADLOCK BÜYÜK PROBLEMDİR. DEADLOCK DA ALTIN KURAL DEAD LOCK A DÜŞMEMEKTİR.
        //BUNUN İÇİN DE BAZI ÇÖZÜMLER VARDIR.
        //1-BUNLARDAN BİRİ BANKER ALGORİTMASI. 
        //2-SENKRONİZASYON NESNELERİNDE BİR TIME OUT OLUR. BUNUNLA PROCESS YÖNETİMİNİ ELE ALIRIZ. DEADLOCKI
        //ÇÖZMEYE ÇALIŞIRIZ OLMADI KAPATIRIZ. ÇÜNKÜ BİR PROGRAMIN KİLİTLENMESİ HOŞ BİR ŞEY DEĞİLDİR.
        //3-PRIORITY(USTÜNLÜK DURUMU): ŞÖYLE İFADE EDEBİLİRİZ BİRBİRLERİNİ KİLİTLEMEMELERİ İÇİN BİRDEN FAZLA
        //DEADLOCK NESNESİNİ AYNI ANDA KULLANMALIYIZ!!EN ÇOK KULLANILAN YÖNTEM BUDUR!!!;
        //  Thread t = new Thread(()=> {
//                lock (l1)
//                {
//                    Console.WriteLine("1");
//                    Thread.Sleep(1000);

//                    lock (l2) Console.WriteLine("2");
//                 });
//              t.Start();
//              lock (l1)
//{
//              Console.WriteLine("3");
//              Thread.Sleep(1000);
//              lock (l2) Console.WriteLine("4");
//}
    }
}




