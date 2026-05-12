export interface CdbCalculationRequest {
  valorInvestido: number;
  meses: number;
}

export interface CdbCalculationResult {
  valorBruto: number;
  valorLiquido: number;
}