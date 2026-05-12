export interface CdbCalculationRequest {
  initialValue: number;
  months: number;
}

export interface CdbCalculationResult {
  grossValue: number;
  netValue: number;
}
