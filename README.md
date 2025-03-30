# ParsingCSHARP

L'attività principale consiste nello sviluppo di un parser per l'elaborazione di documenti PNL (Passenger Name List) utilizzando C# e .NET.

## **Obiettivi**

L’attività prevede lo sviluppo di un parser in C# (preferibilmente .NET 7/8) che:

- Legga e interpreti i documenti PNL forniti.
- Estragga le informazioni rilevanti e le strutturi in oggetti.
- Validi i dati estratti in base a regole specifiche (ad esempio, formati corretti, coerenza dei dati).
- Generi un output in formato JSON.
- Converta gli oggetti JSON in formato PNL (opzionale).
- Confronti i documenti generati con quelli originali (opzionale).

---

## **Panoramica sui formati**

### **Passenger Name List (PNL)**

Una PNL è una lista di passeggeri associata a un volo specifico. Contiene informazioni dettagliate sui passeggeri, inclusi nomi, codici di prenotazione (PNR), dettagli dei bagagli e richieste speciali. Questo documento viene inviato dalla compagnia aerea all’aeroporto di pertinenza.

---

## **Specifiche**

### **Parser PNL**

- **Input:** Documenti PNL in formato testo (forniti in allegato).
- **Output:** Oggetti JSON che rappresentano i dati contenuti nelle PNL.

### **Validazione dei dati**

- Implementare regole di validazione per i dati estratti.

### **Conversione da JSON a PNL (opzionale)**

- **Input:** Oggetti JSON che rappresentano i dati delle PNL.
- **Output:** Documento PNL in formato testo, con struttura e contenuti coerenti con il formato originale.
- **Esito confronto con documenti PNL originali.**

---

## **Dettaglio specifiche**

### **Analisi e Parsing delle PNL**

Il parser deve leggere e analizzare il testo delle PNL, estraendo le seguenti informazioni:

- Dettaglio sul volo (rotta, numero di volo, etc.).
- Dettagli sui passeggeri.
- Dettagli sui bagagli.
- Dettagli dei servizi speciali.

### **Validazione**

- Implementare la validazione dei dati estratti secondo regole definite opportunamente dal candidato.

### **Output JSON**

Ogni record della PNL deve essere convertito in un formato JSON ben strutturato, con le seguenti informazioni:

- Numero Volo.
- Rotta.
- Data.
- Numero passeggeri.
- Nome e cognome dei passeggeri.
- Bagagli.
- Servizi aggiuntivi.

### **Conversione (facoltativa)**

- Implementare la funzionalità per convertire il JSON generato nel formato PNL originale.
- Questa operazione deve garantire che il formato e i contenuti siano fedeli all'originale.

---

## **Dettagli sui campi delle PNL**

### **Struttura della PNL**

#### **Identificativo del volo e data**

Esempio: `PNL NO6149/07SEP RHO`

- **NO6149:** Numero del volo.
- **07SEP:** Data del volo (7 settembre).
- **RHO:** Codice IATA dell'aeroporto di partenza o di arrivo (Rodi).

#### **Informazioni generali sul volo**

Esempio: `MXP312Y`

- **MXP:** Codice IATA dell'aeroporto di destinazione (Malpensa).
- **312Y:** Numero passeggeri e classe (`Y` = economy class).

#### **Dettagli dei passeggeri**

Ogni record di passeggero inizia con un identificativo che segue questo formato:

Esempio: `1COGNOME/NOME-TIPO PASSEGGERO`

- **1ALBANESI/MARCELLOMR** → Nome del passeggero, in questo caso "Marcello Albanesi", identificato come `MR` (Mister, uomo adulto).

#### **Servizi associati ai passeggeri**

- `.R/TOP AL` → Tour operator associato al passeggero (`AL` = Alpitour).
- `.L/655F43` → Identifica il **PNR** (Passenger Name Record), il codice univoco della prenotazione.
- `.R/PDBG HK1 BAGS 01` → Informazioni sui bagagli registrati:
  - `HK1` indica una prenotazione confermata.
  - `BAGS 01` significa che il passeggero ha 1 bagaglio registrato.
- `.R/XBAG HK1 15KG FREE` → Bagaglio extra con 15 kg gratuiti.
- `.R/TKNE HK1 7032015262740/1` → Numero del biglietto elettronico (`TKNE` = e-ticket).

### **Informazioni addizionali**

Alcuni passeggeri possono avere codici speciali associati:

- `.R/CHLD HK1` → Il passeggero è un bambino (`CHLD` = Child).
- `.R/RQST HK1 12A` → Richiesta specifica del passeggero (ad esempio, posto **12A**).

### **Gestione delle incongruenze**

Il parser dovrà essere in grado di riconoscere e interpretare correttamente questi campi per includerli nell'output JSON. Inoltre, eventuali incongruenze nei campi dovranno essere gestite attraverso la validazione dei dati.

---

## **Conclusione**

Questo progetto permette di elaborare documenti PNL con C# e .NET, garantendo estrazione, validazione e trasformazione dei dati in JSON. La conversione inversa e il confronto con documenti originali sono opzionali ma consigliati per un'integrazione completa.
