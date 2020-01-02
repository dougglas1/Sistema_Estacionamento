let buttonSalvar = document.getElementById('salvar');
let modeloVeiculo = document.getElementById('modeloVeiculo');
let placaVeiculo = document.getElementById('placaVeiculo');
let observacao = document.getElementById('observacao');
let modal = document.getElementById('myModal');

// Lista dos dados da tabela
let lista = [];

let linhas = document.getElementById('linhas');
carregaEstacionamento();

buttonSalvar.addEventListener('click', () => {
    inserirEstacionamento();
});

function inserirEstacionamento() {
    //let xml = new XMLHttpRequest();
    //xml.open('POST', '/Estacionamento/EntrarVeiculo?modeloVeiculo=' + modeloVeiculo.value + '&placaVeiculo=' + placaVeiculo.value + '&observacao=' + observacao.value, true);
    //xml.onreadystatechange = () => {
    //    if (xml.readyState === 4 && xml.status === 200) {
    //        alert('Inserido com Sucesso!');
    //        chamaIndex();
    //        carregaEstacionamento();
    //    } else if (xml.status !== 200) {
    //        alert(xml.responseText);
    //    }
    //};
    //xml.send();
    fetch('/Estacionamento/EntrarVeiculo', {
        method: 'POST',
        body: JSON.stringify({
            modelo_veiculo: modeloVeiculo.value,
            placa_veiculo: placaVeiculo.value,
            observacao_veiculo: observacao.value
        }),
        headers: {
            'Content-Type': 'application/json'
        }
    })
        .then(
            alert('Inserido com Sucesso!'),
            chamaIndex()
        )
        .catch(
            alert(xml.responseText)
        );
}

function chamaIndex() {
    modal.style.display = 'none'; // exibir: block
}

function carregaEstacionamento() {
    let xml = new XMLHttpRequest();
    xml.open('GET', '/Estacionamento/BuscarVeiculosEstacionados', true);
    xml.onreadystatechange = () => {
        if (xml.readyState === 4 && xml.status === 200) {
            let data = JSON.parse(xml.responseText);
            console.log(data);

            // Adicionar dados na lista
            lista = data;

            PopularTabela(lista);
        }
    };
    xml.send();
}

function PopularTabela(lista) {
    linhas.innerHTML = '';
    lista.forEach(estacionamento => {
        linhas.innerHTML += `
                                    <tr>
                                        <td>${estacionamento.numero_registro}</td>
                                        <td>${estacionamento.placa_veiculo}</td>
                                        <td>${estacionamento.modelo_veiculo}</td>
                                        <td>${estacionamento.observacao_veiculo}</td>
                                        <td>${estacionamento.data_entrada}</td>
                                        <td>${estacionamento.data_saida}</td>   
                                        <td>
                                            <button class="btn btn-success" onClick='Saida("${estacionamento.numero_registro}")'>
                                                <span class="d-none d-sm-block d-md-block d-lg-block d-xl-block">Saída</span>
                                                <span class="d-block d-sm-none">
                                                    <i class="tim-icons icon-pencil"></i>
                                                </span>
                                            </button>
                                        </td>
                                    </tr>
                                   `
    });
}



function Saida(numero_registro) {
    EfetuarSaida(numero_registro);
}

function EfetuarSaida(numero_registro) {
    let xml = new XMLHttpRequest();
    xml.open('POST', '/Estacionamento/SairVeiculo?codigo=' + numero_registro, true);
    xml.onreadystatechange = () => {
        let data = JSON.parse(xml.responseText);
        if (xml.readyState === 4 && xml.status === 200) {
            alert('Saída com Sucesso! Valor Cobrado: ' + data.value);
            carregaEstacionamento();
        } else if (xml.status !== 200) {
            alert(xml.responseText);
        }
    };
    xml.send();
}
function Filtrar(filtro) {
    if (filtro !== '') {
        PopularTabela(lista.filter(x => x.placa_veiculo.toUpperCase().includes(filtro.toUpperCase()) ||
            x.modelo_veiculo.toUpperCase().includes(filtro.toUpperCase()) ||
            x.observacao_veiculo !== null &&
            x.observacao_veiculo.toUpperCase().includes(filtro.toUpperCase())));
    } else {
        PopularTabela(lista);
    }
}