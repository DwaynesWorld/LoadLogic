import React, { useEffect, useState } from "react";
import parse from "autosuggest-highlight/parse";
import match from "autosuggest-highlight/match";
import useSWR from "swr";
import Autocomplete, {
  AutocompleteRenderInputParams,
  AutocompleteRenderOptionState,
  createFilterOptions
} from "@material-ui/lab/Autocomplete";
import { Typography, TextField } from "@material-ui/core";
import { getAllLocations, LocationsResponse } from "src/api";
import { Location } from "src/models/location";
import { AxiosError, AxiosResponse } from "axios";

interface Props {
  location: Location | null;
  onChange?: (value: Location | null) => void;
}

const filterOptions = createFilterOptions({
  matchFrom: "any",
  limit: 50,
  stringify: (option: Location) => `${option.name} ${option.address1}`
});

export function LocationSelect({ location, onChange }: Props) {
  const [open, setOpen] = useState(false);
  const [options, setOptions] = useState<Location[]>([]);
  const { data, error } = useSWR<AxiosResponse<LocationsResponse>, AxiosError>(
    "/locations",
    getAllLocations
  );

  const loading = open && !error && !data;

  useEffect(() => {
    if (error) {
      return; // Handle
    }

    if (data) {
      setOptions(data.data.locations);
    }
  }, [data, error]);

  function handleOnChange(_: unknown, value: Location | null) {
    if (onChange) onChange(value);
  }

  return (
    <Autocomplete
      data-testid="location-select"
      style={{ width: "100%", minWidth: 250, maxWidth: 350 }}
      open={open}
      onOpen={() => setOpen(true)}
      onClose={() => setOpen(false)}
      options={options}
      loading={loading}
      value={location}
      onChange={handleOnChange}
      filterOptions={filterOptions}
      getOptionSelected={(o, v) => o.id === v.id}
      getOptionLabel={option => option.name}
      renderInput={renderAutocompleteInput}
      renderOption={renderAutocompleteOption}
    />
  );
}

function renderAutocompleteInput(params: AutocompleteRenderInputParams) {
  return (
    <TextField
      {...params}
      placeholder="Choose Location"
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
  const addressMatches = match(option.address1, inputValue);
  const addressParts = parse(option.address1, addressMatches);

  return (
    <div>
      <div>
        {nameParts.map((part, index) => (
          <Typography
            key={index.toString()}
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
            key={index.toString()}
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
