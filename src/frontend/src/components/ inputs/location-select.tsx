/* eslint-disable react/no-array-index-key */
/* eslint-disable @typescript-eslint/no-unsafe-member-access */
/* eslint-disable @typescript-eslint/no-unsafe-call */
/* eslint-disable @typescript-eslint/no-unsafe-assignment */

import React from "react";
import TextField from "@material-ui/core/TextField";
import parse from "autosuggest-highlight/parse";
import match from "autosuggest-highlight/match";
import { Typography } from "@material-ui/core";

import Autocomplete, {
  AutocompleteRenderOptionState,
  createFilterOptions,
} from "@material-ui/lab/Autocomplete";

interface Props {
  onChange?: (value: Location | null) => void;
}

const filterOptions = createFilterOptions({
  matchFrom: "any",
  limit: 50,
  stringify: (option: Location) => `${option.name} ${option.address}`,
});

export function LocationSelect({ onChange }: Props) {
  function handleOnChange(_: unknown, value: Location | null) {
    if (onChange) onChange(value);
  }

  return (
    <Autocomplete
      id="location-select"
      style={{ width: 350 }}
      options={locations}
      onChange={handleOnChange}
      filterOptions={filterOptions}
      getOptionLabel={(option) => option.name}
      renderInput={renderAutocompleteInput}
      renderOption={renderAutocompleteOption}
    />
  );
}

function renderAutocompleteInput(params: unknown) {
  return (
    <TextField
      {...params}
      label="Choose Location"
      variant="outlined"
      size="small"
    />
  );
}

function renderAutocompleteOption(
  option: Location,
  { inputValue }: AutocompleteRenderOptionState
) {
  const nameMatches = match(option.name, inputValue);
  const nameParts = parse(option.name, nameMatches);
  const addressMatches = match(option.address, inputValue);
  const addressParts = parse(option.address, addressMatches);

  return (
    <div>
      <div>
        {nameParts.map((part, index) => (
          <Typography
            key={index}
            variant="body1"
            component="span"
            style={{ fontWeight: part.highlight ? 700 : 400 }}
          >
            {part.text}
          </Typography>
        ))}
      </div>

      <div>
        {addressParts.map((part, index) => (
          <Typography
            key={index}
            variant="body2"
            component="span"
            style={{ fontWeight: part.highlight ? 700 : 400 }}
          >
            {part.text}
          </Typography>
        ))}
      </div>

      <div>
        <Typography
          variant="body2"
          component="span"
          style={{ fontWeight: 400 }}
        >
          {`${option.city}, ${option.state} ${option.zip}`}
        </Typography>
      </div>
    </div>
  );
}

export interface Location {
  contact_first_name: string;
  contact_last_name: string;
  name: string;
  address: string;
  city: string;
  county: string;
  state: string;
  zip: string;
  phone1: string;
  phone2: string;
  contact_email: string;
  web: string;
}

const locations: Location[] = [
  {
    contact_first_name: "Merilyn",
    contact_last_name: "Bayless",
    name: "20 20 Printing Inc",
    address: "195 13n N",
    city: "Santa Clara",
    county: "Santa Clara",
    state: "CA",
    zip: "95054",
    phone1: "408-758-5015",
    phone2: "408-346-2180",
    contact_email: "merilyn_bayless@cox.net",
    web: "http://www.printinginc.com",
  },
  {
    contact_first_name: "Beckie",
    contact_last_name: "Silvestrini",
    name: "A All American Travel Inc",
    address: "7116 Western Ave",
    city: "Dearborn",
    county: "Wayne",
    state: "MI",
    zip: "48126",
    phone1: "313-533-4884",
    phone2: "313-390-7855",
    contact_email: "beckie.silvestrini@silvestrini.com",
    web: "http://www.aallamericantravelinc.com",
  },
  {
    contact_first_name: "Tamar",
    contact_last_name: "Hoogland",
    name: "A K Construction Co",
    address: "2737 Pistorio Rd #9230",
    city: "London",
    county: "Madison",
    state: "OH",
    zip: "43140",
    phone1: "740-343-8575",
    phone2: "740-526-5410",
    contact_email: "tamar@hotmail.com",
    web: "http://www.akconstructionco.com",
  },
  {
    contact_first_name: "Joesph",
    contact_last_name: "Degonia",
    name: "A R Packaging",
    address: "2887 Knowlton St #5435",
    city: "Berkeley",
    county: "Alameda",
    state: "CA",
    zip: "94710",
    phone1: "510-677-9785",
    phone2: "510-942-5916",
    contact_email: "joesph_degonia@degonia.org",
    web: "http://www.arpackaging.com",
  },
  {
    contact_first_name: "Lavera",
    contact_last_name: "Perin",
    name: "Abc Enterprises Inc",
    address: "678 3rd Ave",
    city: "Miami",
    county: "Miami-Dade",
    state: "FL",
    zip: "33196",
    phone1: "305-606-7291",
    phone2: "305-995-2078",
    contact_email: "lperin@perin.org",
    web: "http://www.abcenterprisesinc.com",
  },
  {
    contact_first_name: "Jeanice",
    contact_last_name: "Claucherty",
    name: "Accurel Systems Intrntl Corp",
    address: "19 Amboy Ave",
    city: "Miami",
    county: "Miami-Dade",
    state: "FL",
    zip: "33142",
    phone1: "305-988-4162",
    phone2: "305-306-7834",
    contact_email: "jeanice.claucherty@yahoo.com",
    web: "http://www.accurelsystemsintrntlcorp.com",
  },
  {
    contact_first_name: "Martina",
    contact_last_name: "Staback",
    name: "Ace Signs Inc",
    address: "7 W Wabansia Ave #227",
    city: "Orlando",
    county: "Orange",
    state: "FL",
    zip: "32822",
    phone1: "407-471-6908",
    phone2: "407-429-2145",
    contact_email: "martina_staback@staback.com",
    web: "http://www.acesignsinc.com",
  },
  {
    contact_first_name: "Diane",
    contact_last_name: "Devreese",
    name: "Acme Supply Co",
    address: "1953 Telegraph Rd",
    city: "Saint Joseph",
    county: "Buchanan",
    state: "MO",
    zip: "64504",
    phone1: "816-557-9673",
    phone2: "816-329-5565",
    contact_email: "diane@cox.net",
    web: "http://www.acmesupplyco.com",
  },
  {
    contact_first_name: "Lashandra",
    contact_last_name: "Klang",
    name: "Acqua Group",
    address: "810 N La Brea Ave",
    city: "King of Prussia",
    county: "Montgomery",
    state: "PA",
    zip: "19406",
    phone1: "610-809-1818",
    phone2: "610-378-7332",
    contact_email: "lashandra@yahoo.com",
    web: "http://www.acquagroup.com",
  },
  {
    contact_first_name: "Bulah",
    contact_last_name: "Padilla",
    name: "Admiral Party Rentals & Sales",
    address: "8927 Vandever Ave",
    city: "Waco",
    county: "McLennan",
    state: "TX",
    zip: "76707",
    phone1: "254-463-4368",
    phone2: "254-816-8417",
    contact_email: "bulah_padilla@hotmail.com",
    web: "http://www.admiralpartyrentalssales.com",
  },
  {
    contact_first_name: "Weldon",
    contact_last_name: "Acuff",
    name: "Advantage Martgage Company",
    address: "73 W Barstow Ave",
    city: "Arlington Heights",
    county: "Cook",
    state: "IL",
    zip: "60004",
    phone1: "847-353-2156",
    phone2: "847-613-5866",
    contact_email: "wacuff@gmail.com",
    web: "http://www.advantagemartgagecompany.com",
  },
  {
    contact_first_name: "Chauncey",
    contact_last_name: "Motley",
    name: "Affiliated With Travelodge",
    address: "63 E Aurora Dr",
    city: "Orlando",
    county: "Orange",
    state: "FL",
    zip: "32804",
    phone1: "407-413-4842",
    phone2: "407-557-8857",
    contact_email: "chauncey_motley@aol.com",
    web: "http://www.affiliatedwithtravelodge.com",
  },
  {
    contact_first_name: "Sarah",
    contact_last_name: "Candlish",
    name: "Alabama Educational Tv Comm",
    address: "45 2nd Ave #9759",
    city: "Atlanta",
    county: "Fulton",
    state: "GA",
    zip: "30328",
    phone1: "770-732-1194",
    phone2: "770-531-2842",
    contact_email: "sarah.candlish@gmail.com",
    web: "http://www.alabamaeducationaltvcomm.com",
  },
  {
    contact_first_name: "Benedict",
    contact_last_name: "Sama",
    name: "Alexander & Alexander Inc",
    address: "4923 Carey Ave",
    city: "Saint Louis",
    county: "Saint Louis City",
    state: "MO",
    zip: "63104",
    phone1: "314-787-1588",
    phone2: "314-858-4832",
    contact_email: "bsama@cox.net",
    web: "http://www.alexanderalexanderinc.com",
  },
  {
    contact_first_name: "Laticia",
    contact_last_name: "Merced",
    name: "Alinabal Inc",
    address: "72 Mannix Dr",
    city: "Cincinnati",
    county: "Hamilton",
    state: "OH",
    zip: "45203",
    phone1: "513-508-7371",
    phone2: "513-418-1566",
    contact_email: "lmerced@gmail.com",
    web: "http://www.alinabalinc.com",
  },
  {
    contact_first_name: "Blondell",
    contact_last_name: "Pugh",
    name: "Alpenlite Inc",
    address: "201 Hawk Ct",
    city: "Providence",
    county: "Providence",
    state: "RI",
    zip: "02904",
    phone1: "401-960-8259",
    phone2: "401-300-8122",
    contact_email: "bpugh@aol.com",
    web: "http://www.alpenliteinc.com",
  },
  {
    contact_first_name: "Nu",
    contact_last_name: "Mcnease",
    name: "Amazonia Film Project",
    address: "88 Sw 28th Ter",
    city: "Harrison",
    county: "Hudson",
    state: "NJ",
    zip: "07029",
    phone1: "973-751-9003",
    phone2: "973-903-4175",
    contact_email: "nu@gmail.com",
    web: "http://www.amazoniafilmproject.com",
  },
  {
    contact_first_name: "Moon",
    contact_last_name: "Parlato",
    name: "Ambelang, Jessica M Md",
    address: "74989 Brandon St",
    city: "Wellsville",
    county: "Allegany",
    state: "NY",
    zip: "14895",
    phone1: "585-866-8313",
    phone2: "585-498-4278",
    contact_email: "moon@yahoo.com",
    web: "http://www.ambelangjessicammd.com",
  },
  {
    contact_first_name: "Celeste",
    contact_last_name: "Korando",
    name: "American Arts & Graphics",
    address: "7 W Pinhook Rd",
    city: "Lynbrook",
    county: "Nassau",
    state: "NY",
    zip: "11563",
    phone1: "516-509-2347",
    phone2: "516-365-7266",
    contact_email: "ckorando@hotmail.com",
    web: "http://www.americanartsgraphics.com",
  },
  {
    contact_first_name: "Matthew",
    contact_last_name: "Neither",
    name: "American Council On Sci & Hlth",
    address: "636 Commerce Dr #42",
    city: "Shakopee",
    county: "Scott",
    state: "MN",
    zip: "55379",
    phone1: "952-651-7597",
    phone2: "952-906-4597",
    contact_email: "mneither@yahoo.com",
    web: "http://www.americancouncilonscihlth.com",
  },
  {
    contact_first_name: "Felix",
    contact_last_name: "Hirpara",
    name: "American Speedy Printing Ctrs",
    address: "7563 Cornwall Rd #4462",
    city: "Denver",
    county: "Lancaster",
    state: "PA",
    zip: "17517",
    phone1: "717-491-5643",
    phone2: "717-583-1497",
    contact_email: "felix_hirpara@cox.net",
    web: "http://www.americanspeedyprintingctrs.com",
  },
  {
    contact_first_name: "Joseph",
    contact_last_name: "Cryer",
    name: "Ames Stationers",
    address: "60 Fillmore Ave",
    city: "Huntington Beach",
    county: "Orange",
    state: "CA",
    zip: "92647",
    phone1: "714-584-2237",
    phone2: "714-698-2170",
    contact_email: "joseph_cryer@cox.net",
    web: "http://www.amesstationers.com",
  },
  {
    contact_first_name: "Erinn",
    contact_last_name: "Canlas",
    name: "Anchor Computer Inc",
    address: "13 S Hacienda Dr",
    city: "Livingston",
    county: "Essex",
    state: "NJ",
    zip: "07039",
    phone1: "973-767-3008",
    phone2: "973-563-9502",
    contact_email: "erinn.canlas@canlas.com",
    web: "http://www.anchorcomputerinc.com",
  },
  {
    contact_first_name: "Nelida",
    contact_last_name: "Sawchuk",
    name: "Anchorage Museum Of Hist & Art",
    address: "3 State Route 35 S",
    city: "Paramus",
    county: "Bergen",
    state: "NJ",
    zip: "07652",
    phone1: "201-971-1638",
    phone2: "201-247-8925",
    contact_email: "nelida@gmail.com",
    web: "http://www.anchoragemuseumofhistart.com",
  },
];
