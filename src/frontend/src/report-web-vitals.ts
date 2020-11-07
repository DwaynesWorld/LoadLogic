/* eslint-disable no-console */
import { ReportHandler } from "web-vitals";

const reportWebVitals = async (onPerfEntry?: ReportHandler) => {
  if (onPerfEntry && onPerfEntry instanceof Function) {
    try {
      const vitals = await import("web-vitals");
      vitals.getCLS(onPerfEntry);
      vitals.getFID(onPerfEntry);
      vitals.getFCP(onPerfEntry);
      vitals.getLCP(onPerfEntry);
      vitals.getTTFB(onPerfEntry);
    } catch (error) {
      console.error(error);
    }
  }
};

export default reportWebVitals;
