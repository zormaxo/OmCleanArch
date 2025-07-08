import { Divider, Paper, Typography } from "@mui/material";
import { useLocation } from "react-router-dom";
import type { ProblemDetails } from "../types/api";

export default function ServerError() {
  const { state } = useLocation() as { state: ProblemDetails | null };

  return (
    <Paper>
      {state ? (
        <>
          <Typography gutterBottom variant="h3" sx={{ px: 4, pt: 2 }} color="secondary">
            {state.title || "Server Error"}
          </Typography>
          <Divider />
          <Typography variant="body1" sx={{ p: 4 }}>
            {state.detail || "An unexpected server error occurred."}
          </Typography>
          {state.traceId && (
            <Typography variant="caption" sx={{ px: 4, pb: 2 }} color="text.secondary">
              Trace ID: {state.traceId}
            </Typography>
          )}
        </>
      ) : (
        <Typography variant="h5" gutterBottom sx={{ p: 4 }}>
          Server error
        </Typography>
      )}
    </Paper>
  );
}
