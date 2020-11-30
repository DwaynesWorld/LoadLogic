/* eslint-disable react/no-array-index-key */
/* eslint-disable @typescript-eslint/no-unsafe-member-access */
/* eslint-disable @typescript-eslint/no-unsafe-call */
/* eslint-disable @typescript-eslint/no-unsafe-assignment */

import React, { useEffect, useState } from "react";
import parse from "autosuggest-highlight/parse";
import match from "autosuggest-highlight/match";
import useSWR from "swr";
import Autocomplete, {
  AutocompleteRenderInputParams,
  AutocompleteRenderOptionState,
  createFilterOptions,
} from "@material-ui/lab/Autocomplete";

import {
  Typography,
  TextField,
  InputAdornment,
  Box,
  colors,
} from "@material-ui/core";
import { SearchRounded, AddRounded } from "@material-ui/icons";
import { theme } from "src/theme";
import { getAllCustomers, Customer } from "src/api";

interface Props {
  value: Customer | null;
  onChange?: (value: Customer | null) => void;
  onCreate?: () => void;
}

interface CustomerType extends Customer {
  creation?: boolean;
}

const filterOptions = createFilterOptions({
  matchFrom: "any",
  limit: 50,
  stringify: (option: Customer) =>
    `${option.first_name} ${option.last_name} ${option.email}`,
});

const createOption: CustomerType = {
  creation: true,
  first_name: "",
  last_name: "",
  email: "",
  phone: "",
};

export function CustomerSelect({ value, onChange, onCreate }: Props) {
  const { data, error } = useSWR("/customers", getAllCustomers);

  const [open, setOpen] = useState(false);
  const [options, setOptions] = useState<Customer[]>([]);
  const loading = open && !error && !data;

  useEffect(() => {
    if (error) {
      return; // Handle
    }

    if (data) {
      setOptions([createOption, ...data.customers]);
    }
  }, [data, error]);

  function handleOnChange(_: unknown, newValue: CustomerType | null) {
    if (newValue?.creation && onCreate) onCreate();
    else if (onChange) onChange(newValue);
  }

  return (
    <Autocomplete
      id="customer-select"
      style={{ width: "100%", minWidth: 250, maxWidth: 350 }}
      value={value}
      open={open}
      onOpen={() => setOpen(true)}
      onClose={() => setOpen(false)}
      options={options}
      loading={loading}
      onChange={handleOnChange}
      filterOptions={filterOptions}
      getOptionLabel={(option) => `${option.first_name} ${option.last_name}`}
      renderInput={renderAutocompleteInput}
      renderOption={renderAutocompleteOption}
    />
  );
}

function renderAutocompleteInput(params: AutocompleteRenderInputParams) {
  return (
    <TextField
      {...params}
      placeholder="Search Customers"
      variant="outlined"
      size="small"
      InputProps={{
        ...params.InputProps,
        startAdornment: (
          <InputAdornment position="start">
            <SearchRounded />
          </InputAdornment>
        ),
      }}
    />
  );
}

function renderAutocompleteOption(
  option: CustomerType,
  { inputValue }: AutocompleteRenderOptionState
) {
  if (option.creation) {
    const color = colors.green[700];

    return (
      <Box
        display="flex"
        flexDirection="row"
        alignItems="center"
        flex={1}
        height={40}
      >
        <Box display="flex" flex={1} mr={2}>
          <Typography
            style={{
              fontSize: 14,
              fontWeight: theme.typography.fontWeightBold,
              color,
            }}
          >
            Add a new customer
          </Typography>
        </Box>
        <Box display="flex" alignItems="flex-end">
          <AddRounded htmlColor={color} />
        </Box>
      </Box>
    );
  }

  const fnameMatches = match(option.first_name, inputValue);
  const fnameParts = parse(option.first_name, fnameMatches);

  const lnameMatches = match(option.last_name, inputValue);
  const lnameParts = parse(option.last_name, lnameMatches);

  const emailMatches = match(option.email, inputValue);
  const emailParts = parse(option.email, emailMatches);

  return (
    <div>
      <div>
        {fnameParts.map((part, index) => (
          <Typography
            key={index}
            variant="body1"
            component="span"
            style={{ fontWeight: part.highlight ? 700 : 400 }}
          >
            {part.text}
          </Typography>
        ))}{" "}
        {lnameParts.map((part, index) => (
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
        {emailParts.map((part, index) => (
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
    </div>
  );
}
