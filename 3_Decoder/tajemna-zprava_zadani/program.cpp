#include <iostream>
#include <string>
#include <vector>

const std::string encode_bytes ( std::vector < unsigned char > data ) { //bit shift
    std::vector<std::string> alphabet { //pole písmen a slabik pro šifrování
        "a", "i", "u", "e", "o",
        "ka", "ki", "ku", "ke", "ko",
        "sa", "shi", "su", "se", "so",
        "ta", "chi", "tsu", "te", "to",
        "na", "ni", "nu", "ne", "no",
        "ha", "hi", "fu", "he", "ho",
        "ma", "mi", "mu", "me", "mo",
        "ya", "yu", "yo", "ra", "ri",
        "ru", "re", "ro", "wa",
        "ga", "gi", "gu", "ge", "go",
        "za", "ji", "zu", "ze", "zo",
        "ba", "bi", "bu", "be", "bo",
        "pa", "pi", "pu", "pe", "po" };

    size_t len = data . size ( ); //délka pole
    std::string ret = ""; //output

    unsigned char sum = 0;
    unsigned int pos = 0;
    while ( pos < len ) { //projedu každý prvek
        if ( sum > 64 ) {
            ret += " ";
		}

        sum += data [ pos ]; //přičtu délku aktualniho itemu
        ret += alphabet [ ( data [ pos ] & 0xfc ) >> 2 ];

        if ( pos + 1 < len ) {
            ret += alphabet [ ( ( data [ pos ] & 0x03 ) << 4 ) + ( ( data [ pos + 1 ] & 0xf0 ) >> 4 ) ];

            if ( pos + 2 < len ) {
                ret += alphabet [ ( ( data [ pos + 1 ] & 0x0f ) << 2 ) + ( ( data [ pos + 2 ] & 0xc0 ) >> 6 ) ];
                ret += alphabet [ data [ pos + 2 ] & 0x3f ];
            }
            else {
                ret += alphabet [ ( data [ pos + 1 ] & 0x0f ) << 2 ];
                ret += ".";
            }
        }
        else {
            ret += alphabet [ ( data [ pos + 0 ] & 0x03 ) << 4 ];
            ret += sum > 128 ? "!" : "?";
        }

        pos += 3;
    }

    return ret; //vysledek
}

const std::string encode_string ( const std::string & input ) { //dostanu input
    const unsigned char* input_bytes = ( unsigned char * ) input . c_str ( ); //pole, 1 prvek je balíček bitů reprezentující písmeno// pointer, která si ukládá adresu pole charů (převedeno pomocí funkce c_str, která vrací pointer) ==> mám pointer na pole charů z inputu --> pole bitů

    std::vector < unsigned char > result_bytes; //deklarace výsledného pole charů
    for ( size_t i = 0; i < input . length ( ) - 1; i ++ ) { //projdu celý input kromě posledního prvku
        unsigned char next = input_bytes [ i ] ^ input_bytes [ i + 1 ]; //když jsou oba bity stejné, uloží 0, jinak 1 //porovnávám 2 balíčky z pole
        result_bytes . push_back ( next ); //přidá prvek do výsledného pole
    }
    result_bytes . push_back ( input_bytes [ input . length ( ) - 1 ] ); //přidám do pole poslední prvek ze svého pole, který jsem neporovnávala

    return encode_bytes ( result_bytes );//odešlu svůj text v bináru
}

int main ( ) {
    std::string input = ""; //promenna na budoucí input
    do {
        std::getline ( std::cin, input ); //nacte můj input

        if ( input . length ( ) < 3 ) { //pokud je input mensí, než 3, tak nešifruje
            return 0;
		}

        std::cout << encode_string ( input ) << std::endl; //cout << outputuje input, který přešel přes funkci encode_string << end line (zalomí řádek)
    } while ( true );
}
