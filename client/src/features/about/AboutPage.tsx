import { Alert, AlertTitle, Button, ButtonGroup, Container, List, ListItem, Typography } from "@mui/material";
import {
  useLazyGet400ErrorQuery,
  useLazyGet401ErrorQuery,
  useLazyGet404ErrorQuery,
  useLazyGet500ErrorQuery,
  useLazyGetValidationErrorQuery,
} from "./errorApi";
import { useState } from "react";
import type { ProblemDetails } from "../../app/types/api";

export default function AboutPage() {
  const [validationErrors, setValidationErrors] = useState<string[]>([]);

  const [trigger400Error] = useLazyGet400ErrorQuery();
  const [trigger401Error] = useLazyGet401ErrorQuery();
  const [trigger404Error] = useLazyGet404ErrorQuery();
  const [trigger500Error] = useLazyGet500ErrorQuery();
  const [triggerValidationError] = useLazyGetValidationErrorQuery();

  const getValidationError = async () => {
    const result = await triggerValidationError();
    if (result.error && "data" in result.error) {
      const problemDetails = result.error.data as ProblemDetails;
      if (problemDetails?.errors && Array.isArray(problemDetails.errors)) {
        setValidationErrors(problemDetails.errors.map((err) => err.description || err.code));
      }
    }
  };

  return (
    <Container maxWidth="lg">
      <Typography gutterBottom variant="h3">
        Errors for testing
      </Typography>
      <ButtonGroup fullWidth>
        <Button variant="contained" onClick={() => trigger400Error().catch((err) => console.log(err))}>
          Test 400 Error
        </Button>
        <Button variant="contained" onClick={() => trigger401Error().catch((err) => console.log(err))}>
          Test 401 Error
        </Button>
        <Button variant="contained" onClick={() => trigger404Error().catch((err) => console.log(err))}>
          Test 404 Error
        </Button>
        <Button variant="contained" onClick={() => trigger500Error().catch((err) => console.log(err))}>
          Test 500 Error
        </Button>
        <Button variant="contained" onClick={getValidationError}>
          Test Validation Error
        </Button>
      </ButtonGroup>
      {validationErrors.length > 0 && (
        <Alert severity="error">
          <AlertTitle>Validation errors</AlertTitle>
          <List>
            {validationErrors.map((err) => (
              <ListItem key={err}>{err}</ListItem>
            ))}
          </List>
        </Alert>
      )}
    </Container>
  );
}
