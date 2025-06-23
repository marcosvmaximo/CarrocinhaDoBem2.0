import { Component, OnInit } from '@angular/core';
import { AbstractControl, FormBuilder, FormGroup, FormsModule, Validators } from '@angular/forms';
import { MessageService } from 'primeng/api';
import { Router } from '@angular/router';
import { PetCadastroService } from './pet-cadastro.service';
import { DropdownModule } from 'primeng/dropdown';
import { AutoCompleteModule } from 'primeng/autocomplete';
import { CalendarModule } from 'primeng/calendar';
import { ReactiveFormsModule } from '@angular/forms';
import { ToastModule } from 'primeng/toast';
import { ChipsModule } from 'primeng/chips';
import { InputGroupAddonModule } from 'primeng/inputgroupaddon';
import { InputGroupModule } from 'primeng/inputgroup';
import { InputMaskModule } from 'primeng/inputmask';
import { InputNumberModule } from 'primeng/inputnumber';
import { InputTextModule } from 'primeng/inputtext';
import { InputTextareaModule } from 'primeng/inputtextarea';
import { MultiSelectModule } from 'primeng/multiselect';
import { Ripple } from 'primeng/ripple';
import { FileUploadModule } from 'primeng/fileupload';
import { CommonModule } from '@angular/common';

@Component({
  selector: 'app-pet-cadastro',
  templateUrl: './pet-cadastro.component.html',
  styleUrls: ['./pet-cadastro.component.scss'],
  standalone: true,
  imports: [
    DropdownModule,
    AutoCompleteModule,
    CalendarModule,
    ReactiveFormsModule,
    ToastModule,
    ChipsModule,
    InputGroupAddonModule,
    InputGroupModule,
    InputMaskModule,
    InputNumberModule,
    InputTextModule,
    InputTextareaModule,
    MultiSelectModule,
    FormsModule,
    Ripple,
    FileUploadModule,
    CommonModule
  ],
})

export class PetCadastroComponent {

  name: string = "";

  especies: any[];

  especie: any;

  racas: any[];

  racasAux: any[] = [
    { name: 'Vira-Lata Cão', especie: 'cão' },
    { name: 'Vira-Lata Gato', especie: 'gato' },
    { name: 'Labrador Retriever', especie: 'cão' },
    { name: 'Golden Retriever', especie: 'cão' },
    { name: 'Beagle', especie: 'cão' },
    { name: 'Bulldog Francês', especie: 'cão' },
    { name: 'Poodle', especie: 'cão' },
    { name: 'Bulldog Inglês', especie: 'cão' },
    { name: 'Yorkshire Terrier', especie: 'cão' },
    { name: 'Shih Tzu', especie: 'cão' },
    { name: 'Boxer', especie: 'cão' },
    { name: 'Rottweiler', especie: 'cão' },
    { name: 'Dachshund', especie: 'cão' },
    { name: 'Border Collie', especie: 'cão' },
    { name: 'Chihuahua', especie: 'cão' },
    { name: 'Pug', especie: 'cão' },
    { name: 'Husky Siberiano', especie: 'cão' },
    { name: 'Pastor Alemão', especie: 'cão' },
    { name: 'Doberman', especie: 'cão' },
    { name: 'Basset Hound', especie: 'cão' },
    { name: 'Schnauzer', especie: 'cão' },
    { name: 'Boston Terrier', especie: 'cão' },
    { name: 'Siamês', especie: 'gato' },
    { name: 'Persa', especie: 'gato' },
    { name: 'Maine Coon', especie: 'gato' },
    { name: 'Sphynx', especie: 'gato' },
    { name: 'Ragdoll', especie: 'gato' },
    { name: 'British Shorthair', especie: 'gato' },
    { name: 'Bengal', especie: 'gato' },
    { name: 'Scottish Fold', especie: 'gato' },
    { name: 'Abissínio', especie: 'gato' },
    { name: 'Sagrado da Birmânia', especie: 'gato' },
    { name: 'Shiba Inu', especie: 'cão' },
    { name: 'Maltese', especie: 'cão' },
    { name: 'Dálmata', especie: 'cão' },
    { name: 'Cocker Spaniel', especie: 'cão' },
    { name: 'Setter Irlandês', especie: 'cão' },
    { name: 'Pinscher Miniatura', especie: 'cão' },
    { name: 'Staffordshire Bull Terrier', especie: 'cão' },
    { name: 'Cane Corso', especie: 'cão' },
    { name: 'Shar Pei', especie: 'cão' },
    { name: 'Bulmastife', especie: 'cão' },
    { name: 'Himalaia', especie: 'gato' },
    { name: 'Birmanês', especie: 'gato' },
    { name: 'Tonquinês', especie: 'gato' },
    { name: 'Burmês', especie: 'gato' },
    { name: 'Exótico', especie: 'gato' },
    { name: 'Manx', especie: 'gato' },
    { name: 'Nebelung', especie: 'gato' },
    { name: 'Toyger', especie: 'gato' },
    { name: 'Azul Russo', especie: 'gato' },
    { name: 'Affenpinscher', especie: 'cão' },
    { name: 'Airedale Terrier', especie: 'cão' },
    { name: 'Akita Americano', especie: 'cão' },
    { name: 'Akita Inu', especie: 'cão' },
    { name: 'Malamute-do-alasca', especie: 'cão' },
    { name: 'American Pit Bull Terrier', especie: 'cão' },
    { name: 'American Staffordshire Terrier', especie: 'cão' },
    { name: 'Pastor Australiano', especie: 'cão' },
    { name: 'Boiadeiro Australiano', especie: 'cão' },
    { name: 'Australian Silky Terrier', especie: 'cão' },
    { name: 'Basenji', especie: 'cão' },
    { name: 'Basset Fulvo da Bretanha', especie: 'cão' },
    { name: 'Bearded Collie', especie: 'cão' },
    { name: 'Bedlington Terrier', especie: 'cão' },
    { name: 'Pastor Belga', especie: 'cão' },
    { name: 'Boiadeiro de Berna', especie: 'cão' },
    { name: 'Bichon Frisé', especie: 'cão' },
    { name: 'Bichon Havanês', especie: 'cão' },
    { name: 'Cão de Santo Humberto', especie: 'cão' },
    { name: 'Boiadeiro de Appenzell', especie: 'cão' },
    { name: 'Boiadeiro de Entlebuch', especie: 'cão' },
    { name: 'Borzoi', especie: 'cão' },
    { name: 'Bull Terrier', especie: 'cão' },
    { name: 'Cairn Terrier', especie: 'cão' },
    { name: 'Cão de Canaã', especie: 'cão' },
    { name: 'Cão de Crista Chinês', especie: 'cão' },
    { name: 'Chow Chow', especie: 'cão' },
    { name: 'Clumber Spaniel', especie: 'cão' },
    { name: 'Collie de Pelo Longo', especie: 'cão' },
    { name: 'Coton de Tuléar', especie: 'cão' },
    { name: 'Galgo Escocês', especie: 'cão' },
    { name: 'Dogue Alemão', especie: 'cão' },
    { name: 'Dogue de Bordeaux', especie: 'cão' },
    { name: 'Elkhound Norueguês', especie: 'cão' },
    { name: 'Fox Terrier de Pelo Liso', especie: 'cão' },
    { name: 'Fox Terrier de Pelo Duro', especie: 'cão' },
    { name: 'Flat-Coated Retriever', especie: 'cão' },
    { name: 'Galgo Afegão', especie: 'cão' },
    { name: 'Galgo Espanhol', especie: 'cão' },
    { name: 'Galgo Húngaro', especie: 'cão' },
    { name: 'Galgo Italiano', especie: 'cão' },
    { name: 'Greyhound', especie: 'cão' },
    { name: 'Griffon da Bélgica', especie: 'cão' },
    { name: 'Griffon de Bruxelas', especie: 'cão' },
    { name: 'Hovawart', especie: 'cão' },
    { name: 'Lébrel Irlandês', especie: 'cão' },
    { name: 'Jack Russell Terrier', especie: 'cão' },
    { name: 'Keeshond', especie: 'cão' },
    { name: 'Kerry Blue Terrier', especie: 'cão' },
    { name: 'Komondor', especie: 'cão' },
    { name: 'Kuvasz', especie: 'cão' },
    { name: 'Lakeland Terrier', especie: 'cão' },
    { name: 'Landseer', especie: 'cão' },
    { name: 'Leonberger', especie: 'cão' },
    { name: 'Lhasa Apso', especie: 'cão' },
    { name: 'Manchester Terrier', especie: 'cão' },
    { name: 'Mastife', especie: 'cão' },
    { name: 'Mastim Napolitano', especie: 'cão' },
    { name: 'Mastim Tibetano', especie: 'cão' },
    { name: 'Terra Nova', especie: 'cão' },
    { name: 'Norfolk Terrier', especie: 'cão' },
    { name: 'Norwich Terrier', especie: 'cão' },
    { name: 'Old English Sheepdog', especie: 'cão' },
    { name: 'Papillon', especie: 'cão' },
    { name: 'Parson Russell Terrier', especie: 'cão' },
    { name: 'Pequinês', especie: 'cão' },
    { name: 'Cão do Faraó', especie: 'cão' },
    { name: 'Pointer Inglês', especie: 'cão' },
    { name: 'Lulu da Pomerânia', especie: 'cão' },
    { name: 'Podengo Português', especie: 'cão' },
    { name: 'Rafeiro do Alentejo', especie: 'cão' },
    { name: 'Rhodesian Ridgeback', especie: 'cão' },
    { name: 'Saluki', especie: 'cão' },
    { name: 'Samoieda', especie: 'cão' },
    { name: 'Schipperke', especie: 'cão' },
    { name: 'Scottish Terrier', especie: 'cão' },
    { name: 'Sealyham Terrier', especie: 'cão' },
    { name: 'Setter Gordon', especie: 'cão' },
    { name: 'Skye Terrier', especie: 'cão' },
    { name: 'Soft Coated Wheaten Terrier', especie: 'cão' },
    { name: 'Springer Spaniel Inglês', especie: 'cão' },
    { name: 'São Bernardo', especie: 'cão' },
    { name: 'Terrier Australiano', especie: 'cão' },
    { name: 'Terrier Brasileiro', especie: 'cão' },
    { name: 'Terrier Irlandês', especie: 'cão' },
    { name: 'Terrier Tibetano', especie: 'cão' },
    { name: 'Weimaraner', especie: 'cão' },
    { name: 'Welsh Corgi Cardigan', especie: 'cão' },
    { name: 'Welsh Corgi Pembroke', especie: 'cão' },
    { name: 'Welsh Springer Spaniel', especie: 'cão' },
    { name: 'Welsh Terrier', especie: 'cão' },
    { name: 'West Highland White Terrier', especie: 'cão' },
    { name: 'Whippet', especie: 'cão' },
    { name: 'Cão Lobo Checoslovaco', especie: 'cão' },
    { name: 'Fila Brasileiro', especie: 'cão' },
    { name: 'Grande Boiadeiro Suíço', especie: 'cão' },
    { name: 'Cão da Serra da Estrela', especie: 'cão' },
    { name: 'American Bobtail', especie: 'gato' },
    { name: 'American Curl', especie: 'gato' },
    { name: 'American Shorthair', especie: 'gato' },
    { name: 'American Wirehair', especie: 'gato' },
    { name: 'Australian Mist', especie: 'gato' },
    { name: 'Balinês', especie: 'gato' },
    { name: 'Bombaim', especie: 'gato' },
    { name: 'Chartreux', especie: 'gato' },
    { name: 'Chausie', especie: 'gato' },
    { name: 'Cornish Rex', especie: 'gato' },
    { name: 'Cymric', especie: 'gato' },
    { name: 'Devon Rex', especie: 'gato' },
    { name: 'Don Sphynx', especie: 'gato' },
    { name: 'Mau Egípcio', especie: 'gato' },
    { name: 'Pelo Curto Europeu', especie: 'gato' },
    { name: 'Havana Brown', especie: 'gato' },
    { name: 'Bobtail Japonês', especie: 'gato' },
    { name: 'Javanês', especie: 'gato' },
    { name: 'Korat', especie: 'gato' },
    { name: 'LaPerm', especie: 'gato' },
    { name: 'Lykoi', especie: 'gato' },
    { name: 'Ocicat', especie: 'gato' },
    { name: 'Oriental de Pelo Curto', especie: 'gato' },
    { name: 'Peterbald', especie: 'gato' },
    { name: 'Pixie-bob', especie: 'gato' },
    { name: 'Ragamuffin', especie: 'gato' },
    { name: 'Savannah', especie: 'gato' },
    { name: 'Selkirk Rex', especie: 'gato' },
    { name: 'Serengeti', especie: 'gato' },
    { name: 'Singapura', especie: 'gato' },
    { name: 'Snowshoe', especie: 'gato' },
    { name: 'Sokoke', especie: 'gato' },
    { name: 'Somali', especie: 'gato' },
    { name: 'Thai', especie: 'gato' },
    { name: 'Angorá Turco', especie: 'gato' },
    { name: 'Van Turco', especie: 'gato' },
    { name: 'Africano do Sul', especie: 'gato' },
    { name: 'Chantilly-Tiffany', especie: 'gato' },
    { name: 'Minskin', especie: 'gato' },
    { name: 'Khao Manee', especie: 'gato' },
    { name: 'Dragon Li', especie: 'gato' },
    { name: 'Kurilian Bobtail', especie: 'gato' },
    { name: 'Ojos Azules', especie: 'gato' },
    { name: 'Levkoy Ucraniano', especie: 'gato' },
    { name: 'Braco Alemão de Pelo Curto', especie: 'cão' },
    { name: 'Braco Alemão de Pelo Duro', especie: 'cão' },
    { name: 'Braco Húngaro', especie: 'cão' },
    { name: 'Braco Italiano', especie: 'cão' },
    { name: 'Caniche', especie: 'cão' },
    { name: 'Chesapeake Bay Retriever', especie: 'cão' },
    { name: 'Chinook', especie: 'cão' },
    { name: 'Cão d\'Água Português', especie: 'cão' },
    { name: 'Cão d\'Água Espanhol', especie: 'cão' },
    { name: 'Cão d\'Água Frisado', especie: 'cão' },
    { name: 'Pastor-dos- Pirenéus', especie: 'cão' },
    { name: 'Cão da Montanha dos Pirenéus', especie: 'cão' },
    { name: 'Eurasier', especie: 'cão' },
    { name: 'Pastor Finlandês da Lapônia', especie: 'cão' },
    { name: 'Spitz Finlandês', especie: 'cão' },
    { name: 'Cão-islandês-de-pastoreio', especie: 'cão' },
    { name: 'Jindo Coreano', especie: 'cão' },
    { name: 'Kai Ken', especie: 'cão' },
    { name: 'Kishu Ken', especie: 'cão' },
    { name: 'Lancashire Heeler', especie: 'cão' },
    { name: 'Cão-leopardo-de-Catahoula', especie: 'cão' },
    { name: 'Lundehund norueguês', especie: 'cão' },
    { name: 'Mudi', especie: 'cão' },
    { name: 'Otterhound', especie: 'cão' },
    { name: 'Perdiguero de Burgos', especie: 'cão' },
    { name: 'Petit Basset Griffon Vendéen', especie: 'cão' },
    { name: 'Phalène', especie: 'cão' },
    { name: 'Plott Hound', especie: 'cão' },
    { name: 'Puli', especie: 'cão' },
    { name: 'Pumi', especie: 'cão' },
    { name: 'Pyrenean Shepherd', especie: 'cão' },
    { name: 'Redbone Coonhound', especie: 'cão' },
    { name: 'Pastor-de-brie', especie: 'cão' },
    { name: 'Schapendoes', especie: 'cão' },
    { name: 'Shikoku', especie: 'cão' },
    { name: 'Sloughi', especie: 'cão' },
    { name: 'Stabyhoun', especie: 'cão' },
    { name: 'Sussex Spaniel', especie: 'cão' },
    { name: 'Vallhund Sueco', especie: 'cão' },
    { name: 'Tosa', especie: 'cão' },
    { name: 'Treeing Walker Coonhound', especie: 'cão' },
    { name: 'Xoloitzcuintli', especie: 'cão' },
    { name: 'Yakutian Laika', especie: 'cão' },
    { name: 'Laika da Sibéria Ocidental', especie: 'cão' },
    { name: 'Laika da Sibéria Oriental', especie: 'cão' },
    { name: 'Pastor Romeno de Mioritza', especie: 'cão' },
    { name: 'Cimarrón Uruguayo', especie: 'cão' },
    { name: 'Dogo Argentino', especie: 'cão' },
    { name: 'Dogo Canário', especie: 'cão' },
    { name: 'Dogue de Maiorca', especie: 'cão' },
    { name: 'Pastor-da-Anatólia', especie: 'cão' },
    { name: 'Pastor caucasiano', especie: 'cão' },
    { name: 'Pastor da Ásia Central', especie: 'cão' },
    { name: 'Pastor polonês da planície', especie: 'cão' },
    { name: 'Boerboel', especie: 'cão' },
    { name: 'Barbet', especie: 'cão' },
    { name: 'Azawakh', especie: 'cão' },
    { name: 'Galgo polonês', especie: 'cão' }

  ];

  raca: any;

  sexos: any[];

  sexo: any;

  portes: any[];

  porte: any;

  dataNascimento: any;

  dataResgate: any;

  descricao: any;

  form: FormGroup;

  petImageBinary: any;
  private racaEspecie: string;

  private fieldNames: {
    especie: string;
    name: string;
    raca: string;
    porte: string;
    dataResgate: string;
    sexo: string;
    dataNascimento: string;
    descricao: string
  };

  get f() { return this.form.controls; }

  constructor(private fb: FormBuilder, private msgService: MessageService, private router: Router, private service: PetCadastroService) {
    this.form = this.fb.group({
      name: ['', [Validators.required, Validators.minLength(3), Validators.maxLength(50), Validators.pattern('[a-zA-ZÀ-ÿ\s]{3,50}')]],
      especie: ['', [Validators.required]],
      raca: ['', [Validators.required]],
      sexo: ['', [Validators.required]],
      porte: ['', [Validators.required]],
      dataNascimento: ['', [Validators.required, this.validarData]],
      dataResgate: ['', [Validators.required,  this.validarData]],
      descricao: ['', [Validators.maxLength(100)]],
    });

    this.fieldNames = {
      name: 'Nome',
      especie: 'Espécie',
      raca: 'Raça',
      sexo: 'Sexo',
      porte: 'Porte',
      dataNascimento: 'Data de Nascimento',
      dataResgate: 'Data de Resgate',
      descricao: 'Descrição',
    };

    this.especies = [
      { name: 'Cão', value: "1" },
      { name: 'Gato', value: "2" }
    ];

    this.racas =

    this.sexos = [
      { name: "Macho", value: "1"},
      { name: "Fêmea", value: "2"}
    ]

    this.portes = [
      { name: "Pequeno", value: "1"},
      { name: "Médio", value: "2"},
      { name: "Grande", value: "3"}
    ]
  }

  buscarRacas(event: any) {
    this.racas = this.racasAux;
    this.racas = this.racasAux.filter(raca => raca.especie === this.racaEspecie);

    const filtered: any[] = [];
    const query = event.query;
    for (let i = 0; i < this.racas.length; i++) {
      const raca = this.racas[i];
      if (raca.name.toLowerCase().indexOf(query.toLowerCase()) == 0) {
        filtered.push(raca);
      }
    }

    this.racas = filtered;
  }

  onSubmit() {
    if(this.form.invalid){
      this.showErrors();
      return;
    }
    if(this.f['raca'].valid){
      this.validarRaca();
    }

    if(!this.petImageBinary) {
      this.showErrorViaToast('Imagem do Pet não pode ser vazia.');
      return;
    }

    if(this.form.invalid){
      this.showErrors();
      return;
    }

    const formData = new FormData();
    formData.append('name', this.form.get('name')?.value);
    formData.append('species', this.form.get('especie')?.value.value);
    formData.append('breed', this.form.get('raca')?.value.name);
    formData.append('sex', this.form.get('sexo')?.value.value);
    formData.append('petSize', this.form.get('porte')?.value.value);
    formData.append('birthDate', this.form.get('dataNascimento')?.value.toISOString());
    formData.append('rescueDate', this.form.get('dataResgate')?.value.toISOString());
    formData.append('description', this.form.get('descricao')?.value);
    formData.append('animalPic', this.petImageBinary);
    formData.append('InstitutionId', '1');


    this.service.cadastrarPet(formData).subscribe(
    response => {
      this.msgService.add({ key: 'tst', severity: 'success', summary: 'Mensagem de Erro', detail: 'Pet cadastrado com sucesso!' });
      this.router.navigate(['/dashboard/pets']);
    },
    error => {
      this.showErrorViaToast('Erro ao cadastrar o pet: ' + error);
    }
  );
  }

  validarData(control: AbstractControl){
    const dataEscolhida = new Date(control.value);
    const dataAtual = new Date();

    if(dataEscolhida > dataAtual) {
      return { dataFuture: true };
    }

    return null;
  }

  onChangeEspecie(event: any) {
    if (event) {
      if (event.value === '1') {
        this.racaEspecie = 'cão';
      } else {
        this.racaEspecie = 'gato';
      }

      this.racas = this.racasAux.filter(raca => raca.especie === this.racaEspecie);
    }
  }
  validarRaca(){
    const racaValue = this.f['raca'].value;
    const isRacaValid = this.racas.some(raca => raca === racaValue);

    if(!isRacaValid){
      this.showErrorViaToast('O campo ' + this.fieldNames['raca'] + ' informado, não existe, insira um válido para continuar.');
    }
  }

  showErrors(){
    Object.keys(this.form.controls).forEach(key => {
      // @ts-ignore
      const controlErrors = this.form.get(key).errors;
      if (controlErrors != null) {
        Object.keys(controlErrors).forEach(keyError => {
          switch (keyError) {
            case 'required':
              // @ts-ignore
              this.showErrorViaToast('O campo ' + this.fieldNames[key] + ' é obrigatório.');
              break;
            case 'dataFuture':
               // @ts-ignore
               this.showErrorViaToast('O campo ' + this.fieldNames[key] + ' nâo permite data no futuro.');
               break;
            case 'minlength':
              // @ts-ignore
              this.showErrorViaToast('O valor do campo ' + this.fieldNames[key] + ' é menor do que permitido.');
              break;
            case 'maxlength':
              // @ts-ignore
              this.showErrorViaToast('O valor do campo ' + this.fieldNames[key] + ' é maior do que permitido.');
              break;
            default:
              // @ts-ignore
              this.showErrorViaToast('Erro no campo ' + this.fieldNames[key] + '.');
              break;
          }
        });
      }
    });
  }

  showErrorViaToast(message: string = "") {
    this.msgService.add({ key: 'tst', severity: 'error', summary: 'Mensagem de Erro', detail: message ? message : 'Validação falhou' });
  }

  onBasicUpload() {
    this.msgService.add({ severity: 'info', summary: 'Success', detail: 'File Uploaded with Basic Mode' });
  }

  onFileSelect(event: any){
    if (event.files[0]) {
      const reader = new FileReader();
      reader.onload = (e: any) => {
        const arrayBuffer = e.target.result; // Obtém o array de bytes da imagem
        const blob = new Blob([new Uint8Array(arrayBuffer)], { type: 'image/jpeg' }); // Cria um Blob a partir do array de bytes
        this.petImageBinary = blob; // Atribui ao petImageBinary
      };
      reader.readAsArrayBuffer(event.files[0]);
    }
  }
}
