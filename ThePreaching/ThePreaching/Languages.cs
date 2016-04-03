﻿using System.Collections.Generic;
using System.Linq.Expressions;
using System.Windows.Documents;

namespace ThePreaching
{
    public class Languages
    {
        public Languages()
        {
            LanguagesList = new Dictionary<string, string>()
            {
                {"Dari","224.0.0.118"},
                {"Paschtu","224.0.0.119"},
                {"Arabisch","224.0.0.120"},
                {"Albanisch","224.0.0.121"},
                {"Französisch","224.0.0.122"},
                {"Katalanisch","224.0.0.123"},
                {"Portugiesisch","224.0.0.124"},
                {"Englisch","224.0.0.125"},
                {"Spanisch","224.0.0.126"},
                {"Armenisch","224.0.0.127"},
                {"Russisch","224.0.0.128"},
                {"Aserbaidschanisch","224.0.0.129"},
                {"Amharisch","224.0.0.130"},
                {"Bengalisch","224.0.0.131"},
                {"Niederländisch","224.0.0.132"},
                {"Bhutanisch","224.0.0.133"},
                {"Bosnisch","224.0.0.134"},
                {"Tsuana","224.0.0.135"},
                {"Malaiisch","224.0.0.136"},
                {"Bulgarisch","224.0.0.137"},
                {"Kirundi","224.0.0.138"},
                {"Chinesisch","224.0.0.139"},
                {"Dänisch","224.0.0.140"},
                {"Tigrinja","224.0.0.141"},
                {"Estnisch","224.0.0.142"},
                {"Fidschianisch","224.0.0.143"},
                {"Finnisch","224.0.0.144"},
                {"Schwedisch","224.0.0.145"},
                {"Georgisch","224.0.0.146"},
                {"Griechisch","224.0.0.147"},
                {"Haitianisch","224.0.0.148"},
                {"Hindi","224.0.0.149"},
                {"Indonesisch","224.0.0.150"},
                {"Kurdisch","224.0.0.151"},
                {"Persisch","224.0.0.152"},
                {"Irisch","224.0.0.153"},
                {"Isländisch","224.0.0.154"},
                {"Hebräisch","224.0.0.155"},
                {"Italienisch","224.0.0.156"},
                {"Japanisch","224.0.0.157"},
                {"Kambodschanisch","224.0.0.158"},
                {"Kasachisch","224.0.0.159"},
                {"Suaheli","224.0.0.160"},
                {"Kirgisisch","224.0.0.161"},
                {"Gilbertesisch","224.0.0.162"},
                {"Koreanisch","224.0.0.163"},
                {"Serbisch","224.0.0.164"},
                {"Kroatisch","224.0.0.165"},
                {"Laotisch","224.0.0.166"},
                {"Sotho","224.0.0.167"},
                {"Lettisch","224.0.0.168"},
                {"Deutsch","224.0.0.169"},
                {"Litauisch","224.0.0.170"},
                {"Madagassisch","224.0.0.171"},
                {"Chichewa","224.0.0.172"},
                {"Maledivisch","224.0.0.173"},
                {"Maltesisch","224.0.0.174"},
                {"Marshallesisch","224.0.0.175"},
                {"Mazedonisch","224.0.0.176"},
                {"Rumänisch","224.0.0.177"},
                {"Mongolisch","224.0.0.178"},
                {"Montenegrinisch","224.0.0.179"},
                {"Birmanisch","224.0.0.180"},
                {"Nauruisch","224.0.0.181"},
                {"Nepalesisch","224.0.0.182"},
                {"Maori","224.0.0.183"},
                {"Norwegisch","224.0.0.184"},
                {"Urdu","224.0.0.185"},
                {"Palauisch","224.0.0.186"},
                {"TokPisin(Neomelanesisch)","224.0.0.187"},
                {"Guaraní","224.0.0.188"},
                {"Filipino(Tagalog)","224.0.0.189"},
                {"Polnisch","224.0.0.190"},
                {"Samoanisch","224.0.0.191"},
                {"Rätoromanisch","224.0.0.192"},
                {"RepublikaSrbija","224.0.0.193"},
                {"Tamilisch","224.0.0.194"},
                {"Slowakisch","224.0.0.195"},
                {"Slowenisch","224.0.0.196"},
                {"Somali","224.0.0.197"},
                {"Singhalesisch","224.0.0.198"},
                {"Afrikaans","224.0.0.199"},
                {"Ndebele","224.0.0.200"},
                {"Nordsotho","224.0.0.201"},
                {"Südsotho","224.0.0.202"},
                {"Swasi","224.0.0.203"},
                {"Tsonga","224.0.0.204"},
                {"Venda","224.0.0.205"},
                {"Xhosa","224.0.0.206"},
                {"Zulu","224.0.0.207"},
                {"Tadschikisch","224.0.0.208"},
                {"Thailändisch","224.0.0.209"},
                {"Tetum","224.0.0.210"},
                {"Tongaisch","224.0.0.211"},
                {"Tschechisch","224.0.0.212"},
                {"Türkisch","224.0.0.213"},
                {"Turkmenisch","224.0.0.214"},
                {"Tuvalu(isch)","224.0.0.215"},
                {"Ukrainisch","224.0.0.216"},
                {"Ungarisch","224.0.0.217"},
                {"Usbekisch","224.0.0.218"},
                {"Bislama","224.0.0.219"},
                {"Lateinisch","224.0.0.220"},
                {"NorthernIreland","224.0.0.221"},
                {"Vietnamesisch","224.0.0.222"},
                {"Weißrussisch","224.0.0.223"},
                {"Sango","224.0.0.224"}
            };
        }

        public Dictionary<string, string> LanguagesList;
    }
}