import { TestBed } from '@angular/core/testing';
import { HttpClientTestingModule, HttpTestingController } from '@angular/common/http/testing';
import { CdbService } from './cdb.service';
import { CdbCalculationRequest, CdbCalculationResult } from '../models/cdb.model';
import { environment } from '../../environments/environment';

describe('CdbService', () => {
  let service: CdbService;
  let httpMock: HttpTestingController;
  const apiUrl = `${environment.apiUrl}/api/cdb/calculate`;

  beforeEach(() => {
    TestBed.configureTestingModule({
      imports: [HttpClientTestingModule],
      providers: [CdbService]
    });
    service = TestBed.inject(CdbService);
    httpMock = TestBed.inject(HttpTestingController);
  });

  afterEach(() => {
    httpMock.verify();
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });

  it('should POST to correct URL with request body', () => {
    const request: CdbCalculationRequest = { initialValue: 1000, months: 12 };
    const mockResult: CdbCalculationResult = { grossValue: 1115.68, netValue: 1092.54 };

    service.calculate(request).subscribe(result => {
      expect(result).toEqual(mockResult);
    });

    const req = httpMock.expectOne(apiUrl);
    expect(req.request.method).toBe('POST');
    expect(req.request.body).toEqual(request);
    req.flush(mockResult);
  });

  it('should return observable with calculation result', () => {
    const request: CdbCalculationRequest = { initialValue: 5000, months: 24 };
    const mockResult: CdbCalculationResult = { grossValue: 6234.56, netValue: 5990.12 };

    let actualResult: CdbCalculationResult | undefined;
    service.calculate(request).subscribe(result => {
      actualResult = result;
    });

    const req = httpMock.expectOne(apiUrl);
    req.flush(mockResult);

    expect(actualResult).toEqual(mockResult);
  });
});
