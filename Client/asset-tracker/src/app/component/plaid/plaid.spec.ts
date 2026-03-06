import { ComponentFixture, TestBed } from '@angular/core/testing';

import { Plaid } from './plaid';

describe('Plaid', () => {
  let component: Plaid;
  let fixture: ComponentFixture<Plaid>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [Plaid]
    })
    .compileComponents();

    fixture = TestBed.createComponent(Plaid);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
