import { Component, OnInit } from '@angular/core';
import { PersonService } from 'src/app/services/person.service';
import { FormGroup, FormControl, Validators } from '@angular/forms';

@Component({
  selector: 'app-my-sample',
  templateUrl: './my-sample.component.html',
  styleUrls: ['./my-sample.component.css']
})
export class MySampleComponent implements OnInit {
  myName: string;
  
  yourName: string;
  yourNameResponse: string;
  
  sampleCreateForm: FormGroup;

  nameBackEndErrors: string[];
  ageBackEndErrors: string[];

  isSubmit: boolean = false;

  constructor(
    private personService: PersonService
  ) {
    this.sampleCreateForm = new FormGroup({
      name: new FormControl('', [Validators.required]),
      age: new FormControl('', [Validators.required])
    })
   }

  ngOnInit() {
    this.getMyName();
  }
  // asynchronous
  async getMyName(){
    try {
      let myName = await this.personService.getMyName().toPromise();
      this.myName = myName.message
      console.log(this.myName);
      console.log(myName)
    } catch (error) {
      alert('An error occured')
      console.log(error)
    }
  }

  async submitYourName(){
    try {
      let response = await this.personService.getYourName(this.yourName).toPromise()

      if(response.isSuccess){
        this.yourNameResponse = response.message;
      }
    } catch (error) {
      console.error(error)
    }
  }

  get f() { 
    return this.sampleCreateForm.controls;
  }

  async formSubmit(){
    try {
      let ok = confirm('Are you sure you want to submit?')

      if(!ok) // ok === false
        return;

      if(!this.sampleCreateForm.valid)
        return;

      this.isSubmit = true; // disables button
      this.nameBackEndErrors = null; //resets backEndErrors
      this.ageBackEndErrors = null;

      let result = await this.personService.checkLegalAge(this.sampleCreateForm.value).toPromise();
      if(result.isSuccess){
        alert(result.message);
        this.sampleCreateForm.reset();
      }else {
        alert(result.message);
      }
    } catch (error) {
      console.log(error)
      let errs = error.error;

      if(errs.isSuccess === false){
        alert(errs.message);
        return;
      }

      if(errs.errors){
        if('Name' in errs.errors){
          this.nameBackEndErrors = errs.errors.Name;
        }
        if('Age' in errs.errors){
          this.ageBackEndErrors = errs.errors.Age
        }
      }
    } finally {
      this.isSubmit = false;
    }
  }
 
}
