import { ComponentFixture, TestBed } from '@angular/core/testing';
import { ReactiveFormsModule } from '@angular/forms';
import { HttpClientTestingModule } from '@angular/common/http/testing';
import { of, throwError } from 'rxjs';

import { CdbCalculatorComponent } from './cdb-calculator.component';
import { CdbService } from '../../services/cdb.service';
import { CdbCalculationResult } from '../../models/cdb.model';

describe('CdbCalculatorComponent', () => {
  let component: CdbCalculatorComponent;
  let fixture: ComponentFixture<CdbCalculatorComponent>;
  let cdbServiceSpy: jasmine.SpyObj<CdbService>;

  const mockResult: CdbCalculationResult = { grossValue: 1115.68, netValue: 1092.54 };

  beforeEach(async () => {
    cdbServiceSpy = jasmine.createSpyObj('CdbService', ['calculate']);

    await TestBed.configureTestingModule({
      declarations: [CdbCalculatorComponent],
      imports: [ReactiveFormsModule, HttpClientTestingModule],
      providers: [{ provide: CdbService, useValue: cdbServiceSpy }]
    }).compileComponents();

    fixture = TestBed.createComponent(CdbCalculatorComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });

  it('should initialize form with null values', () => {
    expect(component.calculationForm.get('initialValue')?.value).toBeNull();
    expect(component.calculationForm.get('months')?.value).toBeNull();
  });

  it('should mark form as invalid when empty', () => {
    expect(component.calculationForm.invalid).toBeTrue();
  });

  it('should mark form as invalid when initialValue is 0', () => {
    component.calculationForm.setValue({ initialValue: 0, months: 12 });
    expect(component.calculationForm.invalid).toBeTrue();
  });

  it('should mark form as invalid when months is 1', () => {
    component.calculationForm.setValue({ initialValue: 1000, months: 1 });
    expect(component.calculationForm.invalid).toBeTrue();
  });

  it('should mark form as valid with correct values', () => {
    component.calculationForm.setValue({ initialValue: 1000, months: 12 });
    expect(component.calculationForm.valid).toBeTrue();
  });

  it('should call service on valid form submit', () => {
    cdbServiceSpy.calculate.and.returnValue(of(mockResult));
    component.calculationForm.setValue({ initialValue: 1000, months: 12 });

    component.onSubmit();

    expect(cdbServiceSpy.calculate).toHaveBeenCalledWith({ initialValue: 1000, months: 12 });
  });

  it('should not call service when form is invalid', () => {
    component.onSubmit();
    expect(cdbServiceSpy.calculate).not.toHaveBeenCalled();
  });

  it('should set result on successful calculation', () => {
    cdbServiceSpy.calculate.and.returnValue(of(mockResult));
    component.calculationForm.setValue({ initialValue: 1000, months: 12 });

    component.onSubmit();

    expect(component.result).toEqual(mockResult);
    expect(component.isLoading).toBeFalse();
  });

  it('should set errorMessage on API error', () => {
    cdbServiceSpy.calculate.and.returnValue(throwError(() => new Error('Server error')));
    component.calculationForm.setValue({ initialValue: 1000, months: 12 });

    component.onSubmit();

    expect(component.errorMessage).toBeTruthy();
    expect(component.result).toBeNull();
    expect(component.isLoading).toBeFalse();
  });

  it('should reset form and results on onReset', () => {
    component.result = mockResult;
    component.errorMessage = 'some error';

    component.onReset();

    expect(component.result).toBeNull();
    expect(component.errorMessage).toBeNull();
    expect(component.calculationForm.get('initialValue')?.value).toBeNull();
  });
});
