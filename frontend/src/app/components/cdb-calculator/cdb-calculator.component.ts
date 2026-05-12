import { Component } from '@angular/core';
import { CdbService } from '../../services/cdb.service';
import { CdbCalculationResult } from '../../models/cdb.model';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';

@Component({
  selector: 'app-cdb-calculator',
  templateUrl: './cdb-calculator.component.html',
  styleUrls: ['./cdb-calculator.component.scss']
})
export class CdbCalculatorComponent {
  calculationForm: FormGroup;
  result: CdbCalculationResult | null = null;
  isLoading = false;
  errorMessage: string | null = null;

  constructor(
    private readonly fb: FormBuilder,
    private readonly cdbService: CdbService
  ) {
    this.calculationForm = this.fb.group({
      initialValue: [null, [Validators.required, Validators.min(0.01)]],
      months: [null, [Validators.required, Validators.min(2), Validators.pattern('^[0-9]+$')]]
    });
  }

  get initialValueControl() {
    return this.calculationForm.get('initialValue');
  }

  get monthsControl() {
    return this.calculationForm.get('months');
  }

  onSubmit(): void {
    if (this.calculationForm.invalid) {
      this.calculationForm.markAllAsTouched();
      return;
    }

    this.isLoading = true;
    this.errorMessage = null;
    this.result = null;

    const { initialValue, months } = this.calculationForm.value as { initialValue: number; months: number };

    this.cdbService.calculate({ initialValue, months }).subscribe({
      next: (data) => {
        this.result = data;
        this.isLoading = false;
      },
      error: () => {
        this.errorMessage = 'Ocorreu um erro ao calcular. Verifique os dados e tente novamente.';
        this.isLoading = false;
      }
    });
  }

  onReset(): void {
    this.calculationForm.reset();
    this.result = null;
    this.errorMessage = null;
  }
}
