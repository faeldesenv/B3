/// <reference types="jasmine" />

import { ComponentFixture, TestBed } from '@angular/core/testing';
import { ReactiveFormsModule } from '@angular/forms';
import { of, throwError } from 'rxjs';
import { CdbCalculatorComponent } from './cdb-calculator.component';
import { CdbService } from '../../services/cdb.service';
import { CdbCalculationResult } from '../../models/cdb.model';

describe('CdbCalculatorComponent', () => {
  let component: CdbCalculatorComponent;
  let fixture: ComponentFixture<CdbCalculatorComponent>;
  let cdbServiceMock: jasmine.SpyObj<CdbService>;

  beforeEach(async () => {
    // 1. Criar o Mock do serviço
    cdbServiceMock = jasmine.createSpyObj('CdbService', ['calculate']);

    await TestBed.configureTestingModule({
      imports: [ReactiveFormsModule],
      declarations: [CdbCalculatorComponent],
      providers: [
        { provide: CdbService, useValue: cdbServiceMock }
      ]
    }).compileComponents();

    fixture = TestBed.createComponent(CdbCalculatorComponent);
    component = fixture.componentInstance;
    fixture.detectChanges(); // Aciona o ngOnInit
  });

  it('deve criar o componente', () => {
    expect(component).toBeTruthy();
  });

  it('deve inicializar o formulário vazio e inválido', () => {
    expect(component.form).toBeDefined();
    expect(component.form.valid).toBeFalsy();
  });

  describe('Lógica de Incremento e Decremento', () => {
    it('deve incrementar o número de meses', () => {
      component.monthsCtrl?.setValue(10);
      component.increment();
      expect(component.monthsCtrl?.value).toBe(11);
    });

    it('deve decrementar o número de meses respeitando o limite mínimo de 2', () => {
      component.monthsCtrl?.setValue(3);
      component.decrement();
      expect(component.monthsCtrl?.value).toBe(2);
      
      component.decrement(); // Tenta baixar de 2
      expect(component.monthsCtrl?.value).toBe(2);
    });
  });

  describe('Formatação e Input', () => {
    it('deve formatar valor numérico para BRL corretamente', () => {
      const formatted = component.formatBRL(1250.5);
      // Nota: \xa0 é o espaço não-quebrável que o toLocaleString usa
      expect(formatted).toContain('R$');
      expect(formatted).toContain('1.250,50');
    });

    it('deve retornar a alíquota correta baseada nos meses', () => {
      const cases = [
        { months: 5, expected: '22,5%' },
        { months: 10, expected: '20%' },
        { months: 18, expected: '17,5%' },
        { months: 30, expected: '15%' }
      ];

      cases.forEach(c => {
        component.monthsCtrl?.setValue(c.months);
        expect(component.getTaxLabel()).toBe(c.expected);
      });
    });
  });

  describe('Submissão (Cálculo)', () => {
    const mockResult: CdbCalculationResult = {
      valorBruto: 1100,
      valorLiquido: 1080
    };

    it('deve chamar o serviço quando o formulário for válido', () => {
      cdbServiceMock.calculate.and.returnValue(of(mockResult));

      component.form.setValue({ initialValue: 1000, months: 12 });
      component.onSubmit();

      expect(cdbServiceMock.calculate).toHaveBeenCalledWith({
        valorInvestido: 1000,
        meses: 12
      });
      expect(component.result).toEqual(mockResult);
      expect(component.isLoading).toBeFalse();
    });

    it('deve tratar erro na chamada do serviço', () => {
      const errorResponse = { error: { message: 'Erro na API' } };
      cdbServiceMock.calculate.and.returnValue(throwError(() => errorResponse));

      component.form.setValue({ initialValue: 500, months: 6 });
      component.onSubmit();

      expect(component.errorMessage).toBe('Erro na API');
      expect(component.isLoading).toBeFalse();
    });
  });
});