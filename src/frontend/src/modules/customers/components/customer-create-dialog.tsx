import React from "react";
import Button from "@material-ui/core/Button";
import TextField from "@material-ui/core/TextField";
import Dialog from "@material-ui/core/Dialog";
import DialogActions from "@material-ui/core/DialogActions";
import DialogContent from "@material-ui/core/DialogContent";
import DialogContentText from "@material-ui/core/DialogContentText";
import DialogTitle from "@material-ui/core/DialogTitle";
import Grid from "@material-ui/core/Grid";
import Typography from "@material-ui/core/Typography";
import FormControlLabel from "@material-ui/core/FormControlLabel";
import Checkbox from "@material-ui/core/Checkbox";

import { Customer } from "src/api";
import { Box, makeStyles } from "@material-ui/core";

type CloseReason = "backdropClick" | "escapeKeyDown" | "cancel";

interface Props {
  open: boolean;
  onSaved: (customer: Customer) => void;
  onClose: (event: unknown, reason: CloseReason) => void;
}
export function CustomerCreateDialog({ open, onClose, onSaved }: Props) {
  const styles = useStyles();

  function handleSave() {
    // do something
    onSaved({} as any);
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

      <DialogContent>
        {/* <DialogContentText>
          To subscribe to this website, please enter your email address here. We
          will send updates occasionally.
        </DialogContentText> */}
        <Grid container spacing={3}>
          <Grid item xs={12} sm={4}>
            <TextField
              required
              id="firstName"
              name="firstName"
              label="First name"
              fullWidth
              variant="outlined"
              size="small"
              autoComplete="given-name"
            />
          </Grid>
          <Grid item xs={12} sm={4}>
            <TextField
              required
              id="lastName"
              name="lastName"
              label="Last name"
              fullWidth
              variant="outlined"
              size="small"
              autoComplete="sur-name"
            />
          </Grid>

          <Grid item xs={12} sm={4}>
            <TextField
              required
              id="email"
              name="email"
              label="Email"
              fullWidth
              variant="outlined"
              size="small"
              autoComplete="email"
            />
          </Grid>
        </Grid>

        <Box mt={4}>
          <Typography variant="h5" gutterBottom>
            Shipping address
          </Typography>
        </Box>

        <Grid container spacing={3}>
          <Grid item xs={12}>
            <TextField
              required
              id="address1"
              name="address1"
              label="Address line 1"
              fullWidth
              variant="outlined"
              size="small"
              autoComplete="shipping address-line1"
            />
          </Grid>
          <Grid item xs={12}>
            <TextField
              id="address2"
              name="address2"
              label="Address line 2"
              fullWidth
              variant="outlined"
              size="small"
              autoComplete="shipping address-line2"
            />
          </Grid>
          <Grid item xs={12} sm={6}>
            <TextField
              required
              id="city"
              name="city"
              label="City"
              fullWidth
              variant="outlined"
              size="small"
              autoComplete="shipping address-level2"
            />
          </Grid>
          <Grid item xs={12} sm={6}>
            <TextField
              id="state"
              name="state"
              label="State/Province/Region"
              fullWidth
              variant="outlined"
              size="small"
            />
          </Grid>
          <Grid item xs={12} sm={6}>
            <TextField
              required
              id="zip"
              name="zip"
              label="Zip / Postal code"
              fullWidth
              variant="outlined"
              size="small"
              autoComplete="shipping postal-code"
            />
          </Grid>
          <Grid item xs={12} sm={6}>
            <TextField
              required
              id="country"
              name="country"
              label="Country"
              fullWidth
              variant="outlined"
              size="small"
              autoComplete="shipping country"
            />
          </Grid>
          <Grid item xs={12}>
            <FormControlLabel
              control={
                <Checkbox color="secondary" name="saveAddress" value="yes" />
              }
              label="Use this address for payment details"
            />
          </Grid>
        </Grid>
      </DialogContent>
      <DialogActions>
        <Button onClick={(e) => onClose(e, "cancel")} color="primary">
          Cancel
        </Button>
        <Button onClick={handleSave} color="primary">
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
}));
