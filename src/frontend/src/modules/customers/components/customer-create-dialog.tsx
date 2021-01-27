import React, { useState } from "react";
import Button from "@material-ui/core/Button";
import Dialog from "@material-ui/core/Dialog";
import DialogActions from "@material-ui/core/DialogActions";
import DialogContent from "@material-ui/core/DialogContent";
import DialogTitle from "@material-ui/core/DialogTitle";
import Grid from "@material-ui/core/Grid";
import Typography from "@material-ui/core/Typography";
import FormControlLabel from "@material-ui/core/FormControlLabel";
import Checkbox from "@material-ui/core/Checkbox";
import { Box, colors, makeStyles } from "@material-ui/core";
import { Customer } from "src/models/customer";
import { BaseFormField, InputField } from "src/components";
import { createCustomer } from "src/api/customers";

type CloseReason = "backdropClick" | "escapeKeyDown" | "cancel";

const PERSON_FIELDS: BaseFormField[] = [
  {
    name: "first_name",
    xs: 12,
    sm: 6,
    required: true,
    title: "First Name",
    id: "first_name",
    placeholder: 'ex. "John"',
    autoComplete: "given-name",
  },
  {
    xs: 12,
    sm: 6,
    required: true,
    title: "Last Name",
    id: "last_name",
    name: "last_name",
    placeholder: 'ex. "Doe"',
    autoComplete: "family-name",
  },
  {
    xs: 12,
    title: "Email",
    id: "email",
    name: "email",
    placeholder: 'ex. "john.doe@example.com"',
    autoComplete: "email",
  },
];

const COMPANY_FIELDS: BaseFormField[] = [
  {
    xs: 12,
    sm: 6,
    title: "Company",
    id: "company",
    name: "company",
    placeholder: 'ex. "John Trucking"',
    autoComplete: "shipping name",
  },
  {
    xs: 12,
    sm: 6,
    title: "Phone",
    id: "phone",
    name: "phone",
    placeholder: "###-###-####",
    autoComplete: "phone",
  },
  {
    xs: 12,
    sm: 6,
    title: "Address",
    id: "address1",
    name: "address1",
    placeholder: "Address line 1",
    autoComplete: "shipping address-line1",
  },
  {
    xs: 12,
    sm: 6,
    id: "address2",
    name: "address2",
    title: "Apartment, suite, etc.",
    placeholder: "Address line 2",
    autoComplete: "shipping address-line2",
  },

  {
    xs: 12,
    sm: 6,
    id: "city",
    name: "city",
    title: "City",
    placeholder: "New York",
    autoComplete: "shipping address-level2",
  },
  {
    xs: 12,
    sm: 6,
    id: "state",
    name: "state",
    title: "State",
    placeholder: "State/Province/Region",
  },
  {
    xs: 12,
    sm: 6,
    id: "zip",
    name: "zip",
    placeholder: "Zip / Postal code",
    title: "Zip",
    autoComplete: "shipping postal-code",
  },
  {
    xs: 12,
    sm: 6,
    id: "country",
    name: "country",
    placeholder: "Country",
    title: "Country",
    autoComplete: "shipping country",
  },
];

const DEFAULT_CUSTOMER: Customer = {
  first_name: "",
  last_name: "",
  email: "",
  phone: "",
};

interface Props {
  open: boolean;
  onSaved: (customer: Customer) => void;
  onClose: (event: unknown, reason: CloseReason) => void;
}
export function CustomerCreateDialog({ open, onClose, onSaved }: Props) {
  const styles = useStyles();
  const [isSaving, setIsSaving] = useState(false);
  const [customer, setCustomer] = useState(DEFAULT_CUSTOMER);

  async function handleSave() {
    setIsSaving(true);

    try {
      const response = await createCustomer(customer);
      onSaved(response.data.customer);
    } catch (error) {
      console.error(error);
    }

    setIsSaving(true);
  }

  function handleChange(e: React.ChangeEvent<HTMLInputElement>) {
    setCustomer({ ...customer, [e.target.name]: e.target.value });
  }

  return (
    <Dialog
      open={open}
      onClose={onClose}
      aria-labelledby="form-dialog-title"
      fullWidth
      maxWidth="sm"
      className={styles.dialog}
    >
      <DialogTitle id="form-dialog-title">
        <Typography variant="h3">Create a new customer</Typography>
      </DialogTitle>
      {isSaving && <div>Saving...</div>}

      {!isSaving && (
        <DialogContent>
          <Grid container spacing={3}>
            {PERSON_FIELDS.map((f) => (
              <Grid item xs={f.xs} sm={f.sm}>
                <InputField {...f} showLabel onChange={handleChange} />
              </Grid>
            ))}

            <Grid item xs={12}>
              <FormControlLabel
                control={<Checkbox name="saveAddress" value="yes" />}
                label="Customer is tax exempt"
              />
            </Grid>
          </Grid>

          <Box mt={4} pb={2}>
            <Typography variant="h5" gutterBottom>
              Billing address
            </Typography>
          </Box>

          <Grid container spacing={3}>
            {COMPANY_FIELDS.map((f) => (
              <Grid item xs={f.xs} sm={f.sm}>
                <InputField {...f} showLabel onChange={handleChange} />
              </Grid>
            ))}

            <Grid item xs={12}>
              <FormControlLabel
                control={<Checkbox name="saveAddress" checked />}
                label="Use this address for shipping details"
              />
            </Grid>
          </Grid>
        </DialogContent>
      )}

      <DialogActions>
        <Button
          className={styles.cancelButton}
          onClick={(e) => onClose(e, "cancel")}
          color="primary"
          variant="contained"
        >
          Cancel
        </Button>
        <Button
          className={styles.saveButton}
          onClick={handleSave}
          color="primary"
          variant="contained"
        >
          Save Customer
        </Button>
      </DialogActions>
    </Dialog>
  );
}

const useStyles = makeStyles((theme) => ({
  dialog: {
    marginTop: "82px",
    "& > div.MuiDialog-root": {
      marginTop: "82px",
    },
    "& > div.MuiDialog-scrollPaper": {
      alignItems: "flex-start",
    },
  },
  cancelButton: {
    textTransform: "none",
    fontSize: 15,
    marginLeft: theme.spacing(),
    backgroundColor: colors.common.black,
  },
  saveButton: {
    textTransform: "none",
    fontSize: 15,
    marginLeft: theme.spacing(),
    backgroundColor: colors.green[800],
  },
}));
